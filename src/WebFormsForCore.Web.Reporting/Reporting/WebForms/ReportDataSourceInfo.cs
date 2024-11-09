﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.ReportDataSourceInfo
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

using System;

#nullable disable
namespace Microsoft.Reporting.WebForms
{
  [Serializable]
  public sealed class ReportDataSourceInfo
  {
    private string m_name;
    private string m_prompt;

    internal ReportDataSourceInfo(string name, string prompt)
    {
      this.m_name = name;
      this.m_prompt = prompt;
    }

    public string Name => this.m_name;

    public string Prompt => this.m_prompt;
  }
}
