// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib;
using Microsoft.EntityFrameworkCore.Query;

namespace InfiniLore.Server.Data.Repositories.UserData;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<ILoreScopesRepository>(LifeTime.Scoped)]
public class LoreScopesRepository(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) : ILoreScopesRepository {
    public async Task<Result<IEnumerable<LoreScopeModel>>> GetAllAsync(CancellationToken ct) {
        try {
            return Result<IEnumerable<LoreScopeModel>>.Success(await unitOfWork.Db.LoreScopes
                .Include(ls => ls.Multiverses)
                .ToListAsync(ct));
        }
        catch (Exception e) {
            #if DEBUG 
            return Result<IEnumerable<LoreScopeModel>>.Failure(e.Message);
            #else
            return Result<IEnumerable<LoreScopeModel>>.Failure();
            #endif
        }
    }
    public async Task<Result<IEnumerable<LoreScopeModel>>> GetAllAsync(Func<IQueryable<LoreScopeModel>, IQueryable<LoreScopeModel>> predicate, CancellationToken ct) {
        try {
            IIncludableQueryable<LoreScopeModel, ICollection<MultiverseModel>> query = unitOfWork.Db.LoreScopes
                .Include(ls => ls.Multiverses);
            
            predicate(query);
            
            List<LoreScopeModel> data = await query.ToListAsync(ct);

            return Result<IEnumerable<LoreScopeModel>>.Success(data);
        }
        catch (Exception e) {
            #if DEBUG 
            return Result<IEnumerable<LoreScopeModel>>.Failure(e.Message);
            #else
            return Result<IEnumerable<LoreScopeModel>>.Failure();
            #endif
        }
    }
}
