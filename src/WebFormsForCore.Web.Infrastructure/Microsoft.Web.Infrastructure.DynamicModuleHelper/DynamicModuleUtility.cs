﻿// 
// DynamicModuleUtility.cs
//  
// Author:
//       Marek Habersack <grendel@twistedcode.net>
// 
// Copyright (c) 2011 Novell, Inc (http://novell.com/)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System;
using System.ComponentModel;
using System.Security;
using System.Web;
using System.Web.Configuration;

namespace Microsoft.Web.Infrastructure.DynamicModuleHelper
{
	[EditorBrowsable (EditorBrowsableState.Never)]
	public static class DynamicModuleUtility
	{
		[SecuritySafeCritical]
		public static void RegisterModule (Type moduleType)
		{
			if (moduleType == null)
				return;

			string typeName = moduleType.AssemblyQualifiedName;
			var cfg = WebConfigurationManager.GetWebApplicationSection ("system.web/httpModules") as HttpModulesSection;
			//cfg.Modules.Add (new HttpModuleAction ("__Dynamic_Module_" + typeName, typeName));
		}
	}
}
