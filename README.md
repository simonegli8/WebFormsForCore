# New Repository Location
This project has moved to [https://github.com/webformsforcore/WebFormsForCore](https://github.com/webformsforcore/WebFormsForCore).
The old repository here has been archived.

# WebFormsForCore
A library to run WebForms apps on ASP.NET Core. This library provides a port
of the System.Web libraries of .NET Framework to .NET 8. With this library,
you can run WebForms websites directly in ASP.NET Core, also on Linux.

# Support
If you need support porting your project to NET Core & WebFormsForCore, we provide support for
40$ per hour. Please contact us via [Messenger](https://m.me/simon.jakob.egli).
There is also a tutorial on Youtube on [how to convert a sample WebForms application to
WebFormsForCore](https://youtu.be/Zyb4WBlaUwA).

# Usage
If you have a WebForms project you want to convert to NET Core, proceed as follows:

First convert your Project to a SDK Project. Please keep a backup of the old non SDK style
project. Conversion can be done easiest by using a converter like the migrate-2019 tool. To install that
tool, run `dotnet tool install --global Project2015To2017.Migrate2019.Tool`. Then go to the directory
of your solution and run `dotnet migrate-2019 wizard` to convert your solution to an SDK project. If
the converter complains about an unsupported project type, remove the `<ProjectTypeGuid>` property from
the project first. After conversion change the target framework of your project to `net8.0`. You
might also keep `net48`, in order to dual run your project with NET Framework & NET Core.
Change the OutputPath for `net8.0` to `bin_dotnet`:
```
<PropertyGroup>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
    <AppendRuntimeIdentifierToOutputPath>false</AppendRuntimeIdentifierToOutputPath>
    <!-- Set base intermediate output path, so NET Core build does not conflict with NetFX build. -->
    <BaseIntermediateOutputPath>obj\$(Configuration)\$(TargetFramework)\</BaseIntermediateOutputPath>
</PropertyGroup>

<PropertyGroup Condition="'$(TargetFramework)' != 'net48'">
    <OutputType>Exe</OutputType>
    <OutputPath>bin_dotnet</OutputPath>
    <StartupObject>Program</StartupObject>
</PropertyGroup>

<PropertyGroup Condition="'$(TargetFramework)' == 'net48'">
    <OutputType>Library</OutputType>
    <OutputPath>bin</OutputPath>
</PropertyGroup>
``` 

 Then, for `net8.0`, import the WebFormsForCore packages like so:
```
<ItemGroup Condition="'$(TargetFramework)' == 'net8.0'">
    <PackageReference Include="EstrellasDeEsperanza.WebFormsForCore.Web" Version="1.0.0" />
</ItemGroup>
```
Remove the old `Reference` references or put them in a condition only for `net48`.

If your project also needs `System.Web.Extensions` or `System.Web.Optimization` import the
corresponding packages also, like `EstrellasDeEsperanza.WebFormsForCore.Web.Extensions` or 
`EstrellasDeEsperanza.WebFormsForCore.Web.Optimization` etc. The following packages are available:
- `System.Configuration`: `EstrellasDeEsperanza.WebFormsForCore.Configuration`
- `System.Web`: `EstrellasDeEsperanza.WebFormsForCore.Web`
- `System.Web.Services`: `EstrellasDeEsperanza.WebFormsForCore.Web.Services`
- `System.Web.Extensions`: `EstrellasDeEsperanza.WebFormsForCore.Web.Extensions`
- `System.Web.Optimization`: `EstrellasDeEsperanza.WebFormsForCore.Web.Optimization`
- `System.Web.Mobile`: `EstrellasDeEsperanza.Web.Mobile`
- `Microsoft.AspNet.Web.Optimization.WebForms`: `EstrellasDeEsperanza.WebFormsForCore.Web.Optimization.WebForms`
- `WebGrease`: `EstrellasDeEsperanza.WebFormsForCore.WebGrease`
- `System.Drawing`: `EstrellasDeEsperanza.WebFormsForCore.Drawing`
- `AjaxControlToolkit`: `EstrellasDeEsperanza.WebFormsForCore.AjaxControlToolkit`
- `AjaxControlToolkit.HtmlEditor.Sanitizer`: `EstrellasDeEsperanza.WebFormsForCore.AjaxControlToolkit.HtmlEditor.Sanitizer`
- `AjaxControlToolkit.StaticResources`: `EstrellasDeEsperanza.WebFormsForCore.AjaxControlToolkit.StaticResources`

System.Drawing only implements Attributes, so WebFormsForCore can run on Linux, where System.Drawing.Common.dll is
missing.

If you want WebFormsForCore to automatically create the `*.designer.cs` files for you, as it was in the old non
SDK project, you also need to import the package `EstrellasDeEsperanza.WebFormsForCore.Build`. If you import
this package, outdated `*.designer.cs` files will be created after build. This only works for C#, not for
VisualBasic. Also, the visual designers in VisualStudio for web controls are not supported and won't work.

Finally configure ASP-NET Core to use WebForms in the initialization code Program.cs like so:
```
#if NETCOREAPP

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Program
{

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorPages();
		builder.Services.AddControllersWithViews();

		var app = builder.Build();

		app.UseStaticFiles();

		//app.UseAuthorization();

		//app.MapDefaultControllerRoute();
		
        app.UseWebForms();
			
		app.Run();

	}
}
#endif
```

# Conflicts with Existing Packages
Currently there might be some conflicts with the packages System.Web.dll, System.Drawing.dll &
System.Configuration.ConfigurationManager.dll, since WebFormsForCore replaces those dll's. In order to prevent
import of the old dll's include the following in your csproj:

```
<Target Name="ChangeAliasesOfNugetRefs" BeforeTargets="FindReferenceAssembliesForReferences;ResolveReferences">
    <ItemGroup>
        <!-- Do not import System.Configuration.ConfigurationManager version 8 -->
        <ReferencePath Remove="%(Identity)" Condition="'%(FileName)' == 'System.Configuration.ConfigurationManager' AND $([System.Text.RegularExpressions.Regex]::IsMatch(%(Identity),'\\8[.0-9]+\\'))" />
        <!-- Do not import System.Web -->
        <ReferencePath Remove="%(Identity)" Condition="'%(FileName)' == 'System.Web' AND $([System.Text.RegularExpressions.Regex]::IsMatch(%(Identity),'\\dotnet\\'))" />
        <!-- Do not import System.Drawing -->
        <ReferencePath Remove="%(Identity)" Condition="'%(FileName)' == 'System.Drawing' AND $([System.Text.RegularExpressions.Regex]::IsMatch(%(Identity),'\\dotnet\\'))" />
    </ItemGroup>
</Target>
```
