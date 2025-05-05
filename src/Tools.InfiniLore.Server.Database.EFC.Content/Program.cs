// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;
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

// This is all that is required for EFC to generate the appropriate migrations
ContentDbFactory.RegisterDatabase(builder.Services, 
    static options => options.UseSqlServer(),
    static modelBuilder => modelBuilder
        .ApplyConfigurationsFromAssembly(typeof(CoreAssemblyEntry).Assembly)
        .ApplyConfigurationsFromAssembly(typeof(LoreScopesAssemblyEntry).Assembly)
        .ApplyConfigurationsFromAssembly(typeof(MarkdownFilesAssemblyEntry).Assembly)
        .ApplyConfigurationsFromAssembly(typeof(UsersAssemblyEntry).Assembly)
);

WebApplication app = builder.Build();

app.Run();
