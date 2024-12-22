// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;
using InfiniLore.Server.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace InfiniLore.Database.Repositories.Content.Data.System;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IPermissionsRepository>(ServiceLifetime.Scoped)]
public class PermissionsRepository(IUnitOfWork unitOfWork) : BaseContentRepository<InfiniLorePermission>(unitOfWork), IPermissionsRepository {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override Expression<Func<InfiniLorePermission, bool>> UniqueModelPredicate(InfiniLorePermission originalModel) {
        return model => model.Id == originalModel.Id
            || model.Name == originalModel.Name;
    }
    
    public async ValueTask<RepoResult<InfiniLorePermission[]>> TryGetByNamesAsync(string[] names, CancellationToken ct = default) {
        var dbContext = await _unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        
        InfiniLorePermission[] result = await dbContext.Permissions.Where(p => names.Contains(p.Name)).ToArrayAsync(cancellationToken: ct);
        
        return result;
    }
}
