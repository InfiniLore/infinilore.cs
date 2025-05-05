// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Modules.Core.Database.RepoMethods;

namespace InfiniLore.Server.Modules.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBasicModelRepository<TModel> :
    IUnitOfWorkRepository,
    
    #region CRUD opertations
    IHasAddAsync<TModel>,
    IHasUpdateAsync<TModel>,
    IHasAddOrUpdateAsync<TModel>,
    IHasDeleteAsync<TModel>,
    IHasRemoveAsync<TModel>,
    #endregion
    
    IHasGetByIdAsync<TModel>,
    IHasGetAllAsync<TModel>,
    IHasGetCountAsync,
    IHasIsIdTakenAsync

where TModel : BasicModel;
