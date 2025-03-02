## Intial Creation
[Back][1]

```bash
dotnet new sln -o Apps
cd Apps
dotnet new webapi -n Apps.Api -o Apps\01-Apps.Api
dotnet new classlib -n Apps.Contracts -o Apps\01-Apps.Contracts
dotnet new classlib -n Apps.Infrastructure -o Apps\01-Apps.Infrastructure
dotnet new classlib -n Apps.Application -o Apps\02-Apps.Application
dotnet new classlib -n Apps.Domain -o Apps\03-Apps.Domain
```

```bash
del /s Class1.cs
# touch Apps\01-Apps.Api\Program.cs
# dir/b/s *.csproj
```
## Add all created projects in solution
```bash
  cd Apps
  ## mac
  dotnet sln add (ls -r **\*.csproj)

  ## Windows
  FOR /F %a in ('dir/b/s *.csproj') DO ( dotnet sln add %a )

```
## Build
```bash
#Test Build
dotnet build

```
## Add Project Refernce
```bash
# Add Project reference to Contracts and  Application in Api
dotnet add .\01-Apps.Api reference .\01-Apps.Contracts\ .\02-Apps.Application\

# Add Project reference to Application in Infrastructure
dotnet add .\01-Apps.Infrastructure reference .\02-Apps.Application\

# Add Project reference to Domain in Application
dotnet add .\02-Apps.Application reference .\03-Apps.Domain\

# Add Project reference to Infrastructure in Api ??? not in the drawing
dotnet add .\01-Apps.Api reference .\01-Apps.Infrastructure\
```

## Run
```bash
dotnet run --project 01-Apps.Api
dotnet watch run --project 01-Apps.Api
```
## Test API
[Weather Forcast Test](../Apps/01-Apps.Api/Apps.Api.http)

## Add Package
```bash
dotnet add .\02-Apps.Application package Microsoft.Extensions.DependencyInjection.Abstractions
```

[Edit launchSettings.json](../Apps/01-Apps.Api/Properties/launchSettings.json)
```
"launchBrowser": false
```

delete Apps/01-Apps.Api/Properties/launchSettings.json
[Modify Apps.Api.csproj](../Apps/01-Apps.Api/Apps.Api.csproj) Remove Swashbuckle.AspNetCore and Microsoft.AspNetCore.OpenApi

[Modify Program.cs](../Apps/01-Apps.Api/Program.cs)
```csharp
var builder = WebApplication.CreateBuilder(args);
{
  builder.Services.AddControllers();
}

var app = builder.Build();
{
  // app.UseHttpsRedirection();
  app.MapControllers();
  app.Run();

}
```
> Modify all csproj file to have ItemGroup under PropertyGroup

[Top][0] | [Back to main][1]

[0]:#intial-creation
[1]:../../readme.md