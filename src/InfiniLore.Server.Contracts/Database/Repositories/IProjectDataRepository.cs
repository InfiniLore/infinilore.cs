// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.RepositoryMethods;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IProjectDataRepository<T> :
    IBasicDataRepository<T>,
    IHasGetByLoreScopeAsync<T>
    where T : ProjectData;
