// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.RepositoryMethods;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserDataRepository<T> :
    IBasicDataRepository<T>,
    IHasGetByUserAsync<T>
    where T : UserData;
