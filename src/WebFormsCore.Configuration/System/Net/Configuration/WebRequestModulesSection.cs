﻿// Decompiled with JetBrains decompiler
// Type: System.Net.Configuration.WebRequestModulesSection
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 84F5A18A-F2B1-435C-B86E-09CE162E61E4
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System\v4.0_4.0.0.0__b77a5c561934e089\System.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8.1\System.xml

using System.Configuration;

#nullable disable
namespace System.Net.Configuration
{
	/// <summary>Represents the configuration section for Web request modules. This class cannot be inherited.</summary>
	public sealed class WebRequestModulesSection : ConfigurationSection
	{
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();
		private readonly ConfigurationProperty webRequestModules = new ConfigurationProperty((string)null, typeof(WebRequestModuleElementCollection), (object)null, ConfigurationPropertyOptions.IsDefaultCollection);

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Configuration.WebRequestModulesSection" /> class.</summary>
		public WebRequestModulesSection() => this.properties.Add(this.webRequestModules);

		protected override void PostDeserialize()
		{
			if (this.EvaluationContext.IsMachineLevel)
				return;
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", (object)"webRequestModules"), ex);
			}
		}

		protected internal override void InitializeDefault()
		{
#if !WebFormsCore
			this.WebRequestModules.Add(new WebRequestModuleElement("https:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("http:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("file:", typeof(FileWebRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("ftp:", typeof(FtpWebRequestCreator)));
#endif
		}

		protected internal override ConfigurationPropertyCollection Properties => this.properties;
		/// <summary>Gets the collection of Web request modules in the section.</summary>
		/// <returns>A <see cref="T:System.Net.Configuration.WebRequestModuleElementCollection" /> containing the registered Web request modules.</returns>
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public WebRequestModuleElementCollection WebRequestModules
		{
			get => (WebRequestModuleElementCollection)this[this.webRequestModules];
		}
	}
}
