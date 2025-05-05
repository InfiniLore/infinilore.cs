// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Modules.Core.Database;
using InfiniLore.Server.Modules.LoreScopes.Database;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMarkdownFileRepository>]
public class MarkdownFileRepository : OwnedDataRepository<LoreScopeModel, MarkdownFileModel>, IMarkdownFileRepository {

    public async ValueTask<Result> IsFileNameTakenAsync(string name, Guid loreScopeId, CancellationToken ct = default) {
        if (name.IsNullOrWhiteSpace() || loreScopeId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<MarkdownFileModel> dbSet = GetCachedDbSet<MarkdownFileModel>();

        // Query
        IQueryable<MarkdownFileModel> query = dbSet.Where(l =>
            l.Name == name
            && l.OwnerId == loreScopeId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);
        
    }
    
    public async ValueTask<Result> IsFileNameNotTakenAsync(string name, Guid loreScopeId, CancellationToken ct = default) {
        if (name.IsNullOrWhiteSpace() || loreScopeId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<MarkdownFileModel> dbSet = GetCachedDbSet<MarkdownFileModel>();

        // Query
        IQueryable<MarkdownFileModel> query = dbSet.Where(l =>
            l.Name == name
            && l.OwnerId == loreScopeId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(!result);
    }
}
