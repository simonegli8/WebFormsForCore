﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.IReportViewerMessages3
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

#nullable disable
namespace Microsoft.Reporting.WebForms
{
  public interface IReportViewerMessages3 : IReportViewerMessages2, IReportViewerMessages
  {
    string TotalPages(int pageCount, PageCountMode pageCountMode);

    string CancelLinkText { get; }

    string CalendarLoading { get; }
  }
}
