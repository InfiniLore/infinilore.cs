// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Modules.Core.Server.Database.RepoMethods;

namespace InfiniLore.Modules.Core.Server.Database;
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
