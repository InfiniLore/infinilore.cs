// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using CoreAssemblyEntry = InfiniLore.Server.Modules.Core.IAssemblyEntry;
using LoreScopesAssemblyEntry = InfiniLore.Server.Modules.LoreScopes.IAssemblyEntry;
using MarkdownFilesAssemblyEntry = InfiniLore.Server.Modules.MarkdownFiles.IAssemblyEntry;
using UsersAssemblyEntry = InfiniLore.Server.Modules.Users.IAssemblyEntry;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


var moduleBuilder = ServerModuleBuilder.CreateFromBuilder(builder)
    .AddModule<CoreAssemblyEntry>()
    .AddModule<LoreScopesAssemblyEntry>()
    .AddModule<MarkdownFilesAssemblyEntry>()
    .AddModule<UsersAssemblyEntry>();

// This is all that is required for EFC to generate the appropriate migrations
ContentDbFactory.RegisterDatabase(
    builder.Services,
    moduleBuilder.ModuleAssemblies,
    static options => options.UseSqlServer()
);

WebApplication app = builder.Build();

app.Run();
