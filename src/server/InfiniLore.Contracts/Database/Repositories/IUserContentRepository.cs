// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Contracts.Database.Repositories.RepositoryMethods;
using InfiniLore.Database.Models;

namespace InfiniLore.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserContentRepository<T> :
    IBasicContentRepository<T>,
    IHasTryGetByUserAsync<T>
    where T : UserContent;
