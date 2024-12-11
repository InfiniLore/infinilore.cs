// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using System.Linq.Expressions;

namespace InfiniLore.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class PermissionsRepository(IDbUnitOfWork<MsSqlDbContext> unitOfWork) : BaseContentRepository<InfinilorePermission>(unitOfWork) {

    protected override Expression<Func<InfinilorePermission, bool>> UniqueModelPredicate(InfinilorePermission originalModel) {
        return model => model.Id == originalModel.Id 
            || model.Name == originalModel.Name;
    }
}
