﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution.ListRenderingExtensionsCompletedEventArgs
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution
{
  [DebuggerStepThrough]
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DesignerCategory("code")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public class ListRenderingExtensionsCompletedEventArgs : AsyncCompletedEventArgs
  {
    private object[] results;

    internal ListRenderingExtensionsCompletedEventArgs(
      object[] results,
      Exception exception,
      bool cancelled,
      object userState)
      : base(exception, cancelled, userState)
    {
      this.results = results;
    }

    public Extension[] Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return (Extension[]) this.results[0];
      }
    }
  }
}
