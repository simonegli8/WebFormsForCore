﻿Type.registerNamespace("Microsoft.Reporting.WebFormsClient"),Microsoft.Reporting.WebFormsClient._SessionKeepAlive=function(){Microsoft.Reporting.WebFormsClient._SessionKeepAlive.initializeBase(this),this.KeepAliveUrl=null,this.KeepAliveBody=null,this.KeepAliveIntervalSeconds=0,this.m_keepAliveTimerId=null,this.m_executingKeepAlive=null,this.m_isInitialized=!1,this.m_onTimerIntervalDelegate=Function.createDelegate(this,this.KeepSessionsAlive),this.m_onKeepAliveRequestCompletedDelegate=Function.createDelegate(this,this.OnKeepAliveRequestCompleted),this.m_onAppLoadDelegate=Function.createDelegate(this,this.OnAppLoad)},Microsoft.Reporting.WebFormsClient._SessionKeepAlive.prototype={initialize:function(){Microsoft.Reporting.WebFormsClient._SessionKeepAlive.callBaseMethod(this,"initialize"),Sys.Application.add_load(this.m_onAppLoadDelegate)},dispose:function(){this.CancelKeepAliveTimer(),this.m_executingKeepAlive!=null&&this.m_executingKeepAlive.abort(),delete this.m_onTimerIntervalDelegate,this.m_onTimerIntervalDelegate=null,delete this.m_onKeepAliveRequestCompletedDelegate,this.m_onKeepAliveRequestCompletedDelegate=null,Sys.Application.remove_load(this.m_onAppLoadDelegate),delete this.m_onAppLoadDelegate,this.m_onAppLoadDelegate=null,Microsoft.Reporting.WebFormsClient._SessionKeepAlive.callBaseMethod(this,"dispose")},OnAppLoad:function(){this.m_isInitialized||(this.KeepAliveIntervalSeconds!=0&&(this.KeepSessionsAlive(),this.m_keepAliveTimerId=setInterval(this.m_onTimerIntervalDelegate,this.KeepAliveIntervalSeconds*1e3)),this.m_isInitialized=!0)},KeepSessionsAlive:function(){if(this.m_executingKeepAlive==null){var n=new Sys.Net.WebRequest;n.set_url(this.KeepAliveUrl),n.set_httpVerb("POST"),this.KeepAliveBody!=null&&(n.set_body(this.KeepAliveBody),Sys.Browser.agent!=Sys.Browser.Safari&&(n.get_headers()["Content-Length"]=this.KeepAliveBody.length)),n.add_completed(this.m_onKeepAliveRequestCompletedDelegate),n.invoke(),this.m_executingKeepAlive=n.get_executor()}},OnKeepAliveRequestCompleted:function(n){this.m_executingKeepAlive=null,(n.get_timedOut()||n.get_responseAvailable()&&n.get_statusCode()!=200)&&this.CancelKeepAliveTimer()},CancelKeepAliveTimer:function(){this.m_keepAliveTimerId!=null&&(clearTimeout(this.m_keepAliveTimerId),this.m_keepAliveTimerId=null)}},Microsoft.Reporting.WebFormsClient._SessionKeepAlive.registerClass("Microsoft.Reporting.WebFormsClient._SessionKeepAlive",Sys.Component);