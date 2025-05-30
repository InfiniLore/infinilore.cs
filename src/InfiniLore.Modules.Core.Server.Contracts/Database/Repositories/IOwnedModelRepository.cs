// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Server.Database.RepoMethods;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOwnedModelRepository<TOwner, TModel> :
    IBasicModelRepository<TModel>,
    IHasGetByOwnerAsync<TOwner, TModel>
    where TModel : OwnedModel<TOwner>
    where TOwner : BasicModel;
