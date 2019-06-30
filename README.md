# AuthExample
A .Net Core project with basic authentication between Razor Web Project and WebApi Project using different roles and permissions.

[![Build Status](https://travis-ci.org/Leanwit/AuthExample.svg?branch=master)](https://travis-ci.org/Leanwit/AuthExample)

# Getting Started
All commands described in this section will be executed in root project after clone the repository.
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
WebApi run in https://localhost:5001 as default.


##### Web
```
dotnet run --project src/Web/Web.csproj
```
Web run in https://localhost:1001 as default.

App setting has WebApi urls values to authenticate. You need to update app settings if you change default launch settings.

## Running tests
```
dotnet test
```
Also, it is possible to execute tests per project.
```
dotnet test test/WebApi.Test
dotnet test test/Web.Test
```

To get coverage value using msbuild
```
dotnet test /p:CollectCoverage=true /p:Exclude="[xunit.*]*
```

