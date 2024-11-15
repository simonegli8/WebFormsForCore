﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Reporting.WebForms.WebConfigReader
// Assembly: Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: F82ADCE6-59A1-4E44-AF2B-7E8AD7E2F93B
// Assembly location: C:\Users\simon\Downloads\Microsoft.ReportViewer.WebForms.dll

#nullable disable
namespace Microsoft.Reporting.WebForms
{
  internal sealed class WebConfigReader
  {
    private ConfigFilePropertyInterface<IReportServerConnection> m_serverConnection = new ConfigFilePropertyInterface<IReportServerConnection>("ReportViewerServerConnection", "IReportServerConnection");
    private ConfigFilePropertyInterface<ITemporaryStorage> m_tempStorage = new ConfigFilePropertyInterface<ITemporaryStorage>("ReportViewerTemporaryStorage", "ITemporaryStorage");
    private ConfigFilePropertyInterface<IReportViewerMessages> m_viewerMessages = new ConfigFilePropertyInterface<IReportViewerMessages>("ReportViewerMessages", "IReportViewerMessages");
    private static WebConfigReader m_theInstance;
    private static object m_lockObject = new object();

    private WebConfigReader()
    {
    }

    public static WebConfigReader Current
    {
      get
      {
        lock (WebConfigReader.m_lockObject)
        {
          if (WebConfigReader.m_theInstance == null)
            WebConfigReader.m_theInstance = new WebConfigReader();
          return WebConfigReader.m_theInstance;
        }
      }
    }

    public IReportServerConnection ServerConnection => this.m_serverConnection.GetInstance();

    public ITemporaryStorage TempStorage => this.m_tempStorage.GetInstance();

    public IReportViewerMessages ViewerMessages => this.m_viewerMessages.GetInstance();
  }
}