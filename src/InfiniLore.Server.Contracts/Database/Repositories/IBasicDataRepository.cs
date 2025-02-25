// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.RepositoryMethods;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicDataRepository<T> : 
    IUnitOfWorkRepository, 
    IHasTryAddAsync<T>,
    IHasTryUpdateAsync<T>,
    IHasTryAddOrUpdateAsync<T>,
    IHasTryDeleteAsync<T>,
    IHasTryRemoveAsync<T>,
    IHasTryGetByIdAsync<T>,
    IHasTryGetAllAsync<T>,
    IHasCountAsync

    where T : BasicData;
