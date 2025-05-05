// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.RepositoryMethods;

namespace InfiniLore.Server.Modules.Core.Database.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IOwnedDataRepository<TOwner, TInterface> :
    IBasicDataRepository<TInterface>,
    IHasGetByOwnerAsync<TOwner, TInterface>
    where TInterface : IOwnedData<TOwner>;
