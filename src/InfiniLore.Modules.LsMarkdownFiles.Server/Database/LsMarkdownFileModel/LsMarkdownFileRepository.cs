// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
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
    public ValueTask<RepoOutcome> IsNameTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default) 
        => CommonRepoMethods.IsNameTakenAsync(GetCachedDbSet<LsMarkdownFileModel>(), name, ownerId, notIncludedId, ct);
    
    public ValueTask<RepoOutcome> IsNameNotTakenAsync(string name, Guid ownerId, Guid notIncludedId = default, CancellationToken ct = default)
        => CommonRepoMethods.IsNameNotTakenAsync(GetCachedDbSet<LsMarkdownFileModel>(),name, ownerId, notIncludedId, ct);

    public async ValueTask<RepoOutcome<LsMarkdownFileModel>> GetByNameAndOwnerAsync(string name, Guid ownerId, QueryConfig config = default, CancellationToken ct = default) {
        // Access
        DbSet<LsMarkdownFileModel> dbSet = GetCachedDbSet<LsMarkdownFileModel>();

        // Query
        LsMarkdownFileModel? result = await GetConfiguredQueryable(dbSet, config)
            .Where(ls => ls.Name == name && ls.OwnerId == ownerId)
            .FirstOrDefaultAsync(cancellationToken: ct);

        // Retrieve
        return result is not null 
            ? Outcome.FromData(result) 
            : Outcome.FromError(RepositoryFailures.ModelNotFound);
    }
}
