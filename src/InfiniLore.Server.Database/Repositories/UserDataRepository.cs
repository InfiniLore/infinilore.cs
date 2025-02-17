// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserDataRepository<T> : BasicDataRepository<T>, IUserDataRepository<T> where T : UserData {
    
    
}
