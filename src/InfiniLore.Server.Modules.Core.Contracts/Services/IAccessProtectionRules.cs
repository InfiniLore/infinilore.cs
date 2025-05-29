// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAccessProtectionRules {
    IReadonlyUnitOfWork CreateReadonlyUnitOfWork();
    ValueTask<bool> IsServerAsync(IAccessingUser access);
    ValueTask<bool> IsOwnerAsync(IAccessingUser access, Guid userId);
    
}
