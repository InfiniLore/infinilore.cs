// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BaseOwnedRepository<TModel, TOwner> : BaseModelRepository<TModel> 
    where TModel : BaseOwnedModel<TOwner>
    where TOwner : BaseModel {

    protected override IQueryable<TModel> OptionalInclude(IQueryable<TModel> query) => base.OptionalInclude(query)
        .Include(x => x.Owner)
    ;
}
