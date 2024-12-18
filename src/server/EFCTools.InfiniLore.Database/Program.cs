// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.MsSqlServer;
using Microsoft.EntityFrameworkCore;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<MsSqlDbContext>(options =>
        options.UseSqlServer()
);

WebApplication app = builder.Build();

app.Run();