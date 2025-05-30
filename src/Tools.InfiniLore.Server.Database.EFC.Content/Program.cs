// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.LoreScopes.Server;
using InfiniLore.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(builder)
    .AddModule<IServerModuleEntryCore>()
    .AddModule<IServerModuleEntryLoreScopes>();

builder.Services.RegisterServicesFromInfiniLoreServer();
builder.Services.RegisterServicesFromInfiniLoreShared();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// This is all that is required for EFC to generate the appropriate migrations
ContentDbFactory.RegisterDatabase(
    builder.Services,
    moduleBuilder.ModuleAssemblies,
    static options => options.UseSqlServer()
);

S3FileDbFactory.RegisterDatabase(
    builder.Services,
    "localhost",
    "minioadmin",
    "minioadmin"
);

WebApplication app = builder.Build();

app.Run();
