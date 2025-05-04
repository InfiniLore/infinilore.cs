// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Database.RepositoryMethods;

namespace InfiniLore.Server.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicDataRepository<T> :
    IUnitOfWorkRepository,
    
    #region CRUD opertations
    IHasAddAsync<T>,
    IHasUpdateAsync<T>,
    IHasAddOrUpdateAsync<T>,
    IHasDeleteAsync<T>,
    IHasRemoveAsync<T>,
    #endregion
    
    IHasGetByIdAsync<T>,
    IHasGetAllAsync<T>,
    IHasGetCountAsync,
    IHasIsIdTakenAsync
    where T : IBasicData;
