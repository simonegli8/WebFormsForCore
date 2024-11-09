﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.ReportParameterSupplier
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Reporting.WebForms
{
  internal class ReportParameterSupplier : IParameterSupplier
  {
    private Report m_report;

    public ReportParameterSupplier(Report report) => this.m_report = report;

    public bool IsReadyForConnection => this.m_report.IsReadyForConnection;

    public bool IsQueryExecutionAllowed
    {
      get => !(this.m_report is ServerReport report) || report.IsQueryExecutionAllowed();
    }

    public ReportParameterInfoCollection GetParameters() => this.m_report.GetParameters();

    public void SetParameters(IEnumerable<ReportParameter> parameters)
    {
      this.m_report.SetParameters(parameters);
    }

    public ReportDataSourceInfoCollection GetDataSources(out bool allCredentialsSatisfied)
    {
      if (this.m_report is ServerReport report1)
        return report1.GetDataSources(out allCredentialsSatisfied);
      LocalReport report2 = (LocalReport) this.m_report;
      if (report2.SupportsQueries)
        return report2.GetDataSources(out allCredentialsSatisfied);
      allCredentialsSatisfied = true;
      return (ReportDataSourceInfoCollection) null;
    }

    public void SetDataSourceCredentials(DataSourceCredentialsCollection credentials)
    {
      if (this.m_report is ServerReport report)
        report.SetDataSourceCredentials((IEnumerable<DataSourceCredentials>) credentials);
      else
        ((LocalReport) this.m_report).SetDataSourceCredentials((IEnumerable) credentials);
    }
  }
}
