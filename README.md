# AuthExample
.Net Core project example with basic authentication between Razor Web Project and WebApi Project using different roles and permissions.

[![Build status](https://ci.appveyor.com/api/projects/status/lc6do2hmf10ew2ca?svg=true)](https://ci.appveyor.com/project/Leanwit/authexample)

# Getting Started
All commands described in this section will be executed in root project after cloning the repository.
## Prerequisites
[.Net Core 2.2](https://dotnet.microsoft.com/download)

## Installing
```
dotnet build
```

## How to run
##### WebApi
```
dotnet run --project src/WebApi/WebApi.csproj
```
WebApi run in https://localhost:5001 by default.

##### Web
```
dotnet run --project src/Web/Web.csproj
```
Web run in https://localhost:1000 by default.

To log in, you can see all users and password created by default in [_src/WebApi/Infrastructure/Persistence/Seed/UserDataGenerator.cs_](https://github.com/Leanwit/AuthExample/blob/master/src/WebApi/Infrastructure/Persistence/Seed/UserDataGenerator.cs).

Examples:
* admin:admin
* pageone:pageone

## Running tests
```
dotnet test
```
It is also possible to execute tests per project.
```
dotnet test test/WebApi.Test
dotnet test test/Web.Test
```

Get coverage value using msbuild.
```
dotnet test /p:CollectCoverage=true /p:Exclude="[xunit.*]*
```
or you can check the last build [here](https://ci.appveyor.com/project/Leanwit/authexample).
# Next steps
[Issue List](https://github.com/Leanwit/AuthExample/issues)
