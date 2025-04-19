// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Data.Project;
using InfiniLore.Server.Database.Models.Data.Project;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Database.Repositories.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IMarkdownFileRepository>()]
public class MarkdownFileRepository : ProjectDataRepository<MarkdownFile>, IMarkdownFileRepository {

    public async ValueTask<Result> IsFileNameTakenAsync(string name, Guid loreScopeId, CancellationToken ct = default) {
        if (name.IsNullOrWhiteSpace()) return Result.FromError(RepositoryFailures.ModelFailedValidation);
        if (loreScopeId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

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
        if (name.IsNullOrWhiteSpace()) return Result.FromError(RepositoryFailures.ModelFailedValidation);
        if (loreScopeId == Guid.Empty) return Result.FromError(RepositoryFailures.ModelFailedValidation);

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
