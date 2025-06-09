// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules;
using InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Modules.LoreScopes.Server;
using InfiniLore.Modules.LoreScopes.Shared;
using InfiniLore.Modules.LsMarkdownFiles.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ModuleProvider moduleProvider = ModuleProviderBuilder.Create(builder.Services)
    .AddModule<ModuleSetupCoreServer>()
    .AddModule<ModuleSetupLoreScopesServer>()
    .AddModule<ModuleSetupLsMarkdownFilesServer>()
    .Build();

builder.Services.RegisterServicesFromInfiniLoreServer();
builder.Services.RegisterServicesFromInfiniLoreModulesLoreScopesShared();
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
    moduleProvider.GetAssemblies(),
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
