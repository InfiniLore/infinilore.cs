// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;
using Old.InfiniLore.Database;

namespace InfiniLore.Server.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class BasicDataRepository<T> : UnitOfWorkRepository<ContentDbContext>, IBasicDataRepository<T> where T : BasicData {
    
    
}
