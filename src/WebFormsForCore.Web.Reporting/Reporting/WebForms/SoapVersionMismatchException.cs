﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.SoapVersionMismatchException
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Microsoft.Reporting.WebForms
{
  [Serializable]
  public sealed class SoapVersionMismatchException : ReportServerException
  {
    internal SoapVersionMismatchException(string message, Exception innerException)
      : base(message, (string) null, innerException)
    {
    }

    private SoapVersionMismatchException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
