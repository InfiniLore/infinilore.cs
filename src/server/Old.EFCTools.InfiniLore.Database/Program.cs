// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<ContentDbContext>(options =>
        options.UseSqlServer()
);

builder.Services.AddIdentityCore<InfiniLoreUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ContentDbContext>()
    .AddSignInManager()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>();

builder.Services.AddIdentityApiEndpoints<InfiniLoreUser>();

WebApplication app = builder.Build();

app.Run();