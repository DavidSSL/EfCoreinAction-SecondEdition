At the moment on Linux we have to use .NET 5.0 (7 Aug 2022) because if we use .NET 6.0, we run into the fact that Linux is not yet supported.

Anyway, the following information is based upon this [post](https://www.phillipsj.net/posts/working-with-sql-server-on-linux-for-dotnet-development/) but it is adapted and some of the information is reproduced below.

Anyway, here are is what I had to do. 

## Install dB

I used Docker to install MS SQL because on Linux [we don't have localDB](https://github.com/dotnet/efcore/issues/6336#issuecomment-240857551). 

```shell
docker run -e 'ACCEPT_EULA=Y' --user 'root' -e 'SA_PASSWORD=YOUR_OWN_PASSWORD' -p 1433:1433 -v ~/MsSqlData/2019/data:/var/opt/mssql/data -v ~/MsSqlData/2019/log:/var/opt/mssql/log -v ~/MsSqlData/2019/secrets:/var/opt/mssql/secrets -d --name 'MSSql2019-dev' mcr.microsoft.com/mssql/server:2019-latest

```

Then in the various connection strings in the solution, you will have to change them to:

```csharp
private const string ConnectionString = @"Server=localhost;Database=MyFirstDb;User Id=sa;Password=YOUR_OWN_PASSWORD"; 
```

## Store Connection string in user secrets

In order to avoid inadvertently committing your secret password to your VCS consider using the [User Secrets tool](https://github.com/aspnet/DotNetTools). This *does not* encrypt the data though.

For this to work we need to import the following packages:

```nuget
dotnet add pakcage Microsoft.Extensions.Configuration
dotnet add pakcage Microsoft.Extensions.Configuration.Binder
dotnet add pakcage Microsoft.Extensions.Configuration.Json
dotnet add pakcage Microsoft.Extensions.Configuration.UserSecrets
```

Then create the connection string in User Secrets:

```shell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=MyFirstDb;User Id=sa;Password=yourStrongPassword;" --id 9F5F9E14-4FF7-4F3A-AC46-0D5A165E2544
```

Then in your project file add the `UserSecretsId`:

```csharp
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <LangVersion>9</LangVersion>
    <UserSecretsId>9F5F9E14-4FF7-4F3A-AC46-0D5A165E2544</UserSecretsId>
    <TargetFramework>net5.0</TargetFramework>
  </PropertyGroup>
```

where the `UserSecretsId` matches the value of `--id`. 

This value is stored in `~/.microsoft/usersecrets/9F5F9E14-4FF7-4F3A-AC46-0D5A165E2544/secrets.json`.

## Running migrations

In order to run migrations, you'll need to do:

```shell
dotnet add package Microsoft.EntityFrameworkCore.Design
```
