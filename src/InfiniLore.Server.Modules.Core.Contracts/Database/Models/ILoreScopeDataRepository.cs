// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.Database.RepositoryMethods;

namespace InfiniLore.Server.Modules.Core.Database.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopeDataRepository<T> :
    IBasicDataRepository<T>,
    IHasGetByLoreScopeAsync<T> where T : ILoreScopeData;
