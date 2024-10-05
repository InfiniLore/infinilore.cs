// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database;
using InfiniLore.Server.Database.Models.UserData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace InfiniLore.Server.API.Controllers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[ApiController]
[Route("[controller]")]
public class LoreScopeController(ILogger logger, IServiceProvider serviceProvider ) : InfiniLoreControllerBase(serviceProvider) {
    [HttpGet(Name = "GetLoreScopes")]
    public async Task<IEnumerable<LoreScopeModel>> Get() {
        await using InfiniLoreDbContext dbContext = await GetDbContext();

        List<LoreScopeModel> data = await dbContext.LoreScopes.ToListAsync();
        return data;
    }
}
