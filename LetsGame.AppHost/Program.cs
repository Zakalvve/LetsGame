using Aspire.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.Configuration["sqlpassword"];

if (builder.Environment.IsDevelopment() && String.IsNullOrEmpty(sqlPassword))
{
    throw new InvalidOperationException(
        """
        A password for the local SQL Server container is not configured.
        Add one to the AppHost project's user secrets with the key 'sqlpassword', e.g. dotnet user-secrets set sqlpassword <password>
        """
    );
}

// Components
var sqlserver = builder.AddSqlServerContainer("sql-server", sqlPassword, 6969)
    .WithVolumeMount("VolumeMount.sqlServer.data", "/var/opt/mssql", VolumeMountType.Named);

// Databases
var identityDb = sqlserver.AddDatabase("IdentityDb");
var coreDb = sqlserver.AddDatabase("CoreDB");

// Microservices
var identityApi = builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(identityDb);

var coreApi = builder.AddProject<Projects.Core_API>("core-api")
    .WithReference(coreDb)
    .WithEnvironmentForServiceBinding("IdentityUrl", identityApi);

// Apps
var webApp = builder.AddProject<Projects.BlazorApp>("web-app")
    .WithReference(coreApi)
    .WithEnvironmentForServiceBinding("IdentityUrl", identityApi)
    .WithLaunchProfile("https");

webApp.WithEnvironmentForServiceBinding("CallBackUrl", webApp, bindingName: "https");

identityApi
    .WithEnvironmentForServiceBinding("CoreApi", coreApi)
    .WithEnvironmentForServiceBinding("WebAppClient", webApp, bindingName: "https");

builder.Build().Run();
