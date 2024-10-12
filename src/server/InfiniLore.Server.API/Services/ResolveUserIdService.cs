// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.API.Dto;
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;

namespace InfiniLore.Server.API.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IResolveUserIdService>(LifeTime.Scoped)]
public class ResolveUserIdService(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) : IResolveUserIdService {
    private readonly InfiniLoreDbContext _dbContext = unitOfWork.GetDbContext();

    public async Task<InfiniLoreUser?> ResolveUserIdAsync<T>(T hasUserId, CancellationToken ct) where T : IRequiresUserId => await _dbContext.Users.FindAsync([hasUserId.UserId.ToString()], ct);
}
