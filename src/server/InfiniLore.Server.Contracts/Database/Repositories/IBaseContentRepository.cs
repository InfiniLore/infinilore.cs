// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;

namespace InfiniLore.Server.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBaseContentRepository<T> :
    IHasTryAddAsync<T>,
    IHasTryUpdateAsync<T>,
    IHasTryAddOrUpdateAsync<T>,
    IHasTryDeleteAsync<T>,
    IHasTryRemoveAsync<T>,
    IHasTryGetByIdAsync<T>,
    IHasTryGetAllAsync<T>,
    IHasTryGetByCriteriaAsync<T>,
    IHasCountAsync,
    IRepository
    where T : BaseContent;
