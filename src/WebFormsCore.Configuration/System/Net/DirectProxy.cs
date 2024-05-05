﻿// Decompiled with JetBrains decompiler
// Type: System.Net.DirectProxy
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 84F5A18A-F2B1-435C-B86E-09CE162E61E4
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System\v4.0_4.0.0.0__b77a5c561934e089\System.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8.1\System.xml

#nullable disable
namespace System.Net
{
  internal class DirectProxy : ProxyChain
  {
    private bool m_ProxyRetrieved;

    internal DirectProxy(Uri destination)
      : base(destination)
    {
    }

    protected override bool GetNextProxy(out Uri proxy)
    {
      proxy = (Uri) null;
      if (this.m_ProxyRetrieved)
        return false;
      this.m_ProxyRetrieved = true;
      return true;
    }
  }
}
