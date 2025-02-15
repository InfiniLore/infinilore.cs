// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Contracts.Database.Repositories.RepositoryMethods;
using InfiniLore.Database.Models;

namespace InfiniLore.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicContentRepository<T> :
    IHasTryAddAsync<T>,
    IHasTryUpdateAsync<T>,
    IHasTryAddOrUpdateAsync<T>,
    IHasTryDeleteAsync<T>,
    IHasTryRemoveAsync<T>,
    IHasTryGetByIdAsync<T>,
    IHasTryGetAllAsync<T>,
    IHasCountAsync,
    IUnitOfWorkRepository
    where T : BasicContent;
