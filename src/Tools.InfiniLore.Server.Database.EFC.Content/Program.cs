// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;
using Microsoft.EntityFrameworkCore;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// This is all that is required for EFC to generate the appropriate migrations
ContentDbFactory.RegisterDatabase(builder.Services, optionsAction: options =>
    options.UseSqlServer()
);

WebApplication app = builder.Build();

app.Run();
