// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Server;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<ILsMarkdownFileRepository>]
public class LsMarkdownFileRepository : OwnedModelRepository<LoreScopeModel, LsMarkdownFileModel>, ILsMarkdownFileRepository {

    protected override IQueryable<LsMarkdownFileModel> AlwaysInclude(IQueryable<LsMarkdownFileModel> query) 
        => base.AlwaysInclude(query)
        .Include(l => l.S3FileMetaData);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ValueTask<Result> IsNameTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default) 
        => CommonRepoMethods.IsNameTakenAsync(GetCachedDbSet<LsMarkdownFileModel>(), name, ownerId, notIncludedId, ct);
    
    public ValueTask<Result> IsNameNotTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default)
        => CommonRepoMethods.IsNameNotTakenAsync(GetCachedDbSet<LsMarkdownFileModel>(),name, ownerId, notIncludedId, ct);

    public async ValueTask<Result<LsMarkdownFileModel>> GetByNameAndOwnerAsync(string name, Guid ownerId, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<LsMarkdownFileModel> dbSet = GetCachedDbSet<LsMarkdownFileModel>();

        // Query
        LsMarkdownFileModel? result = await GetConfiguredQueryable(dbSet, config)
            .Where(ls => ls.Name == name && ls.OwnerId == ownerId)
            .FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        return result is not null 
            ? Result<LsMarkdownFileModel>.FromSuccess(result) 
            : Result<LsMarkdownFileModel>.FromError(RepositoryFailures.ModelNotFound);
    }
}
