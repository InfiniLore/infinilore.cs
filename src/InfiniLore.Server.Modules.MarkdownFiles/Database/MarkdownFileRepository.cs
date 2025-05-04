// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Contracts.Database;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Modules.MarkdownFiles.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMarkdownFileRepository>()]
public class MarkdownFileRepository : ProjectDataRepository<MarkdownFile>, IMarkdownFileRepository {

    public async ValueTask<Result> IsFileNameTakenAsync(string name, Guid loreScopeId, CancellationToken ct = default) {
        if (name.IsNullOrWhiteSpace() || loreScopeId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<MarkdownFile> dbSet = GetCachedDbSet<MarkdownFile>();

        // Query
        IQueryable<MarkdownFile> query = dbSet.Where(l =>
            l.Name == name
            && l.LoreScopeId == loreScopeId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(result);
        
    }
    
    public async ValueTask<Result> IsFileNameNotTakenAsync(string name, Guid loreScopeId, CancellationToken ct = default) {
        if (name.IsNullOrWhiteSpace() || loreScopeId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

        // Access
        DbSet<MarkdownFile> dbSet = GetCachedDbSet<MarkdownFile>();

        // Query
        IQueryable<MarkdownFile> query = dbSet.Where(l =>
            l.Name == name
            && l.LoreScopeId == loreScopeId
        );

        // Retrieve
        bool result = await query.AnyAsync(cancellationToken: ct);
        return Result.FromState(!result);
    }
}
