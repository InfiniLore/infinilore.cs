// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;

namespace InfiniLore.Modules.Core.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAccessProtectionRules {
    IReadonlyUnitOfWork CreateReadonlyUnitOfWork();
    ValueTask<bool> IsServerAsync(IAccessingUser access, CancellationToken ct = default);
    ValueTask<bool> IsOwnerAsync(IAccessingUser access, Guid userId, CancellationToken ct = default);
    
}
