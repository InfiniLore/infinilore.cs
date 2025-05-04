// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Database.RepositoryMethods;

namespace InfiniLore.Server.Modules.Core.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicDataRepository<TInterface> :
    IUnitOfWorkRepository,
    
    #region CRUD opertations
    IHasAddAsync<TInterface>,
    IHasUpdateAsync<TInterface>,
    IHasAddOrUpdateAsync<TInterface>,
    IHasDeleteAsync<TInterface>,
    IHasRemoveAsync<TInterface>,
    #endregion
    
    IHasGetByIdAsync<TInterface>,
    IHasGetAllAsync<TInterface>,
    IHasGetCountAsync,
    IHasIsIdTakenAsync

where TInterface : IBasicData;
