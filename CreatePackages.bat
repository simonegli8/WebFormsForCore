SET PackageVersion=1.1.1-beta
SET Configuration=Debug

REM msbuild /p:Configuration=%Configuration%

cd  src
cd WebFormsForCore.Compilers
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Build
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Configuration
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Drawing
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Serialization.Formatters
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web
dotnet pack WebFormsForCore.Web.csproj --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web.ApplicationServices
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web.Extensions
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web.Infrastructure
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web.Optimization
dotnet pack WebFormsForCore.Web.Optimization.csproj --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web.Optimization.WebForms
dotnet pack --include-symbols -o ..\..\nupkg --no-build -c %Configuration%
cd ..\WebFormsForCore.Web.RegularExpressions
dotnet pack --include-symbols -o ..\..\nupkg --no-build
cd ..\WebFormsForCore.Web.Services
dotnet pack --include-symbols -o ..\..\nupkg --no-build
cd ..\WebFormsForCore.WebGrease
dotnet pack WebFormsForCore.WebGrease.csproj --include-symbols -o ..\..\nupkg --no-build
cd ..\..