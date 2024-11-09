﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.SearchState
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

using System;

#nullable disable
namespace Microsoft.Reporting.WebForms
{
  [Serializable]
  public sealed class SearchState
  {
    private string m_text;
    private int m_startPage;

    internal SearchState(string text, int startPage)
    {
      this.m_text = text;
      this.m_startPage = startPage;
    }

    public string Text => this.m_text;

    public int StartPage => this.m_startPage;
  }
}
