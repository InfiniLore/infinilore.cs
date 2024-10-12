// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.API;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class InfiniLoreControllerBase(IServiceProvider provider) : Controller {
    private readonly IDbContextFactory<InfiniLoreDbContext> _factory = provider.GetRequiredService<IDbContextFactory<InfiniLoreDbContext>>();

    protected Task<InfiniLoreDbContext> GetDbContext() => _factory.CreateDbContextAsync();
}
