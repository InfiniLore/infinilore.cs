// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoreAssemblyEntry = InfiniLore.Server.Modules.Core.IAssemblyEntry;
using LoreScopesAssemblyEntry = InfiniLore.Server.Modules.LoreScopes.IAssemblyEntry;
using UsersAssemblyEntry = InfiniLore.Server.Modules.Users.IAssemblyEntry;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(builder)
    .AddModule<CoreAssemblyEntry>()
    .AddModule<LoreScopesAssemblyEntry>()
    .AddModule<UsersAssemblyEntry>();

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

WebApplication app = builder.Build();

app.Run();
