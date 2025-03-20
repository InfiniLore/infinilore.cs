// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;

namespace InfiniLore.Server.Services.Auth0;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CredentialsStore(CredentialsFlags.ToLowerCaseInvariant, true, "/")]
public static partial class InfiniLoreClaimsStore {
    private const string Prefix = "https://infinilore.dev";
    [Prefix(Prefix)]public static partial string UserId { get; }
    [Prefix(Prefix)]public static partial string UserName { get; }
}
