// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace InfiniLore.Database.Repositories.Content.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IPermissionsRepository>(ServiceLifetime.Scoped)]
public class PermissionsRepository(IUnitOfWork unitOfWork) : BaseContentRepository<InfinilorePermission>(unitOfWork), IPermissionsRepository {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override Expression<Func<InfinilorePermission, bool>> UniqueModelPredicate(InfinilorePermission originalModel) {
        return model => model.Id == originalModel.Id
            || model.Name == originalModel.Name;
    }
    
    public async ValueTask<RepoResult> AllPermissionNamesIncludedAsync(string[] permissionNames, CancellationToken ct = default) {
        var dbContext = await _unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        
        HashSet<string> foundPermissionNames = await dbContext.Permissions
            .Where(p => permissionNames.Contains(p.Name))
            .Select(p => p.Name) // Project only the names to minimize data transfer
            .ToHashSetAsync(ct);
        
        if (!permissionNames.All(foundPermissionNames.Contains)) return "Not all permissions were found";
        return true;
    }
}
