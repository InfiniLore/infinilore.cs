// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;

namespace InfiniLore.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CredentialsStore(CredentialsFlags.ToLowerCaseInvariant, true, "/")]
public static partial class InfiniLoreClaimsStore {
    private const string UrlPrefix = "https://claims.infinilore.dev";

    [Prefix(UrlPrefix)] public static partial string UserId { get; }
    [Prefix(UrlPrefix)] public static partial string UserName { get; }
}
