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
    public const string FromClaims = nameof(FromClaims);
    public const string FromJwtToken = nameof(FromJwtToken);
    public const string FromServer = nameof(FromServer);
}
