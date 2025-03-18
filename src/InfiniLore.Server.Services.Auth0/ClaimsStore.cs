// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;

namespace InfiniLore.Server.Services.Auth0;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CredentialsStore(CredentialsFlags.ToLowerCaseInvariant, true, "_")]
public static partial class ClaimsStore {
    public static partial string InfiniloreUserId { get; }
    public static partial string InfiniloreUserName { get; }
}
