// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Contracts.Data.Repositories;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Data.Repositories.UserData;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<ILoreScopesRepository>(LifeTime.Scoped)]
public class LoreScopesRepository(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) : ILoreScopesRepository {
    #region DeleteAsync
    public async Task<Result<bool>> DeleteAsync(Guid loreScopeId, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();

        LoreScopeModel? loreScope = await dbContext.LoreScopes.FindAsync([loreScopeId], ct);
        if (loreScope is null) return Result<bool>.Failure($"LoreScope with id {loreScopeId} not found");

        return await DeleteAsync(loreScope, ct);
    }

    public async Task<Result<bool>> DeleteAsync(LoreScopeModel model, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();

        model.SoftDelete();
        await dbContext.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
    #endregion
}
