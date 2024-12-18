// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<MsSqlDbContext>(options =>
        options.UseSqlServer()
);


builder.Services.AddIdentityCore<InfiniLoreUser>(options => {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<MsSqlDbContext>();

WebApplication app = builder.Build();

app.Run();