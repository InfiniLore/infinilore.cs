// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;

namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAccessProtectionRules>]
public class AccessProtectionRules(IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory) : IAccessProtectionRules{
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IReadonlyUnitOfWork CreateReadonlyUnitOfWork() => readonlyUnitOfWorkFactory.Create();
    public ValueTask<bool> IsServerAsync(IAccessingUser access, CancellationToken ct = default) => ValueTask.FromResult(
        access.UserId == Guid.Empty
        && access.MetaData.TryGetValue("server", out object? server)
        && server is true
    );
    
    public ValueTask<bool> IsOwnerAsync(IAccessingUser access, Guid userId, CancellationToken ct = default) => ValueTask.FromResult(
        access.UserId == userId
    );
}
