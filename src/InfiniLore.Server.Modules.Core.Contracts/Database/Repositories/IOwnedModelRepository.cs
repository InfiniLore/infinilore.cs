// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.RepoMethods;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOwnedModelRepository<TOwner, TModel> :
    IBasicModelRepository<TModel>,
    IHasGetByOwnerAsync<TOwner, TModel>
    where TModel : OwnedModel<TOwner>
    where TOwner : BasicModel;
