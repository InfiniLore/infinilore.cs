// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models;
using InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;

namespace InfiniLore.Server.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserContentRepository<T> :
    IBasicContentRepository<T>,
    IHasTryGetByUserAsync<T>
    where T : UserContent;
