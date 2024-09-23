//------------------------------------------------------------------------------
// <copyright file="CompilationLock.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

//#define MUTEXINSTRUMENTATION

namespace System.Web.Compilation
{

	using System;
	using System.Threading;
	using System.Globalization;
	using System.Configuration;
	using System.Security.Principal;
	using System.Web.Util;
	using System.Web.Configuration;
	using System.Runtime.InteropServices;
	using System.Web.Management;
	using System.Runtime.Versioning;
	using System.Diagnostics;
	using Debug = System.Web.Util.Debug;

	internal sealed class CompilationMutex : IDisposable
	{

		private String _name;
		private String _comment;
#if MUTEXINSTRUMENTATION
    // Used to keep track of the stack when the mutex is obtained
    private string _stackTrace;
#endif

		// ROTORTODO: replace unmanaged aspnet_isapi mutex with managed implementation
#if NETFRAMEWORK && !FEATURE_PAL // No unmanaged aspnet_isapi mutex in Coriolis
    private HandleRef   _mutexHandle;
#else
		private Mutex _mutex;
#endif // !FEATURE_PAL
		// Lock Status is used to drain out all worker threads out of Mutex ownership on
		// app domain shutdown: -1 locked for good, 0 unlocked, N locked by a worker thread(s)
		private int _lockStatus;
		private bool _draining = false;

		internal CompilationMutex(String name, String comment)
		{
			string mutexRandomName = null;

			if (OSInfo.IsWindows)
			{
				// Attempt to get the mutex string from the registry (VSWhidbey 415795)
				mutexRandomName = (string)Misc.GetAspNetRegValue("CompilationMutexName",
					null /*valueName*/, null /*defaultValue*/);
			}

			if (mutexRandomName != null)
			{
				// If we were able to use the registry value, use it.  Also, we need to prepend "Global\"
				// to the mutex name, to make sure it can be shared between a terminal server session
				// and IIS (VSWhidbey 307523).
				_name += @"Global\" + name + "-" + mutexRandomName;
			}
			else
			{
				// If we couldn't get the reg value, don't use it, and prepend "Local\" to the mutex
				// name to make it local to the session (and hence prevent hijacking)
				_name += @"Local\" + name;
			}

			_comment = comment;

			Debug.Trace("Mutex", "Creating Mutex " + MutexDebugName);

#if NETFRAMEWORK && !FEATURE_PAL // No unmanaged aspnet_isapi mutex in Coriolis
        _mutexHandle = new HandleRef(this, UnsafeNativeMethods.InstrumentedMutexCreate(_name));

        if (_mutexHandle.Handle == IntPtr.Zero) {
            Debug.Trace("Mutex", "Failed to create Mutex " + MutexDebugName);

            throw new InvalidOperationException(SR.GetString(SR.CompilationMutex_Create));
        }

        Debug.Trace("Mutex", "Successfully created Mutex " + MutexDebugName);
#else
			_mutex = new Mutex(false, _name);
#endif // !FEATURE_PAL
		}

		~CompilationMutex()
		{
			Close();
		}

		void IDisposable.Dispose()
		{
			Close();
			System.GC.SuppressFinalize(this);
		}

		internal /*public*/ void Close()
		{

#if NETFRAMEWORK && !FEATURE_PAL // No unmanaged aspnet_isapi mutex in Coriolis

        if (_mutexHandle.Handle != IntPtr.Zero) {
            UnsafeNativeMethods.InstrumentedMutexDelete(_mutexHandle);
            _mutexHandle = new HandleRef(this, IntPtr.Zero);
        }
#else
			_mutex?.Dispose();
#endif // !FEATURE_PAL
		}

		[ResourceExposure(ResourceScope.None)]
		internal /*public*/ void WaitOne()
		{

#if NETFRAMEWORK && !FEATURE_PAL // No unmanaged aspnet_isapi mutex in Coriolis
        if (_mutexHandle.Handle == IntPtr.Zero)
            throw new InvalidOperationException(SR.GetString(SR.CompilationMutex_Null));
#endif

        // check the lock status
        for (;;) {
            int lockStatus = _lockStatus;

            if (lockStatus == -1 || _draining)
                throw new InvalidOperationException(SR.GetString(SR.CompilationMutex_Drained));

            if (Interlocked.CompareExchange(ref _lockStatus, lockStatus+1, lockStatus) == lockStatus)
                break; // got the lock
        }

        Debug.Trace("Mutex", "Waiting for mutex " + MutexDebugName);

#if NETFRAMEWORK && !FEATURE_PAL
		if (UnsafeNativeMethods.InstrumentedMutexGetLock(_mutexHandle, -1) == -1) {
            // failed to get the lock
            Interlocked.Decrement(ref _lockStatus);
            throw new InvalidOperationException(SR.GetString(SR.CompilationMutex_Failed));
        }
#else
			_mutex.WaitOne();
#endif

#if MUTEXINSTRUMENTATION
        // Remember the stack trace for debugging purpose
        _stackTrace = (new StackTrace()).ToString();
#endif

			Debug.Trace("Mutex", "Got mutex " + MutexDebugName);
		}

		internal /*public*/ void ReleaseMutex()
		{

#if NETFRAMEWORK && !FEATURE_PAL // No unmanaged aspnet_isapi mutex in Coriolis
			if (_mutexHandle.Handle == IntPtr.Zero)
				throw new InvalidOperationException(SR.GetString(SR.CompilationMutex_Null));
#endif
			Debug.Trace("Mutex", "Releasing mutex " + MutexDebugName);

#if NETCOREAPP // No unmanaged aspnet_isapi mutex in Coriolis
			_mutex.ReleaseMutex();
#endif

#if MUTEXINSTRUMENTATION
			// Clear out the stack trace
			_stackTrace = null;
#endif

#if NETFRAMEWORK && !FEATURE_PAL // No unmanaged aspnet_isapi mutex in Coriolis
			if (UnsafeNativeMethods.InstrumentedMutexReleaseLock(_mutexHandle) != 0)
            Interlocked.Decrement(ref _lockStatus);
#endif
		}


		private String MutexDebugName
		{
			get
			{
#if DBG
            return (_comment != null) ? _name + " (" + _comment + ")" : _name;
#else
				return _name;
#endif
			}
		}
	}

	internal static class CompilationLock
	{

		private static CompilationMutex _mutex;

		static CompilationLock()
		{

			// Create the mutex (or just get it if another process created it).
			// Make the mutex unique per application
			int hashCode = StringUtil.GetNonRandomizedHashCode("CompilationLock" + HttpRuntime.AppDomainAppId.ToLower(CultureInfo.InvariantCulture));


			_mutex = new CompilationMutex(
							"CL" + hashCode.ToString("x", CultureInfo.InvariantCulture),
							"CompilationLock for " + HttpRuntime.AppDomainAppVirtualPath);
		}

		internal static void GetLock(ref bool gotLock)
		{

			// The idea of this try/finally is to make sure that the statements are always
			// executed together (VSWhidbey 319154)
			// This code should be using a constrained execution region.
			try
			{
			}
			finally
			{
				// Always take the BuildManager lock *before* taking the mutex, to avoid possible
				// deadlock situations (VSWhidbey 530732)
#pragma warning disable 0618
				//@TODO: This overload of Monitor.Enter is obsolete.  Please change this to use Monitor.Enter(ref bool), and remove the pragmas   -- Microsoft
				Monitor.Enter(BuildManager.TheBuildManager);
#pragma warning restore 0618
				_mutex.WaitOne();
				gotLock = true;
			}
		}

		internal static void ReleaseLock()
		{
			_mutex.ReleaseMutex();
			Monitor.Exit(BuildManager.TheBuildManager);
		}

	}

}
