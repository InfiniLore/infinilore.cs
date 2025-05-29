// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Modules.Core;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMessageBroker {
    ValueTask<IAccessingUser> GetMessageAccessAsync(CancellationToken ct = default);
    
    // Added here because it is kinda required by all those who use it as keys for the services.
    public const string Claims = nameof(Claims);
    public const string JwtToken = nameof(JwtToken);
    public const string Server = nameof(Server);
}
