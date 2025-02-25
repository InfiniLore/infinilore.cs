// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Database.Models;

namespace InfiniLore.Server.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class SystemDataRepository<T> : BasicDataRepository<T>, ISystemDataRepository<T> where T : SystemData {
}
