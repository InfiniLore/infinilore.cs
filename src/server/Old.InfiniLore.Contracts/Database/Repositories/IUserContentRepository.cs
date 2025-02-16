// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models;
using Old.InfiniLore.Contracts.Database.Repositories.RepositoryMethods;

namespace Old.InfiniLore.Contracts.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserContentRepository<T> :
    IBasicContentRepository<T>,
    IHasTryGetByUserAsync<T>
    where T : UserContent;
