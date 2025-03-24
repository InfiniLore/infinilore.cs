// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;
using System.Collections.Frozen;

namespace InfiniLore.Server.Services.Auth0;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CredentialsStore(CredentialsFlags.IterateValues, true, ".")]
public static partial class RolesStore {
    
    public static Lazy<FrozenDictionary<string, string[]>> PermissionsPerRoles { get; } = new(static () => new Dictionary<string, string[]> {
        [User] = UserPermissions.Value,
        [Consumer] = ConsumerPermissions.Value,
        [Producer] = ProducerPermissions.Value,
        [InfiniloreAdmin] = InfiniloreAdminPermissions.Value,
        [InfiniloreDeveloper] = InfiniloreDeveloperPermissions.Value
    }.ToFrozenDictionary());

    // -----------------------------------------------------------------------------------------------------------------
    // Assignments
    // -----------------------------------------------------------------------------------------------------------------
    public static partial string User { get; }
    public static Lazy<string[]> UserPermissions { get; } = new(() => [
        PermissionsStoreConstants.LorescopeRead
    ]);

    public static partial string Consumer { get; }
    public static Lazy<string[]> ConsumerPermissions { get; } = new(() => [
        ..UserPermissions.Value
    ]);

    public static partial string Producer { get; }
    public static Lazy<string[]> ProducerPermissions { get; } = new(() => [
        ..ConsumerPermissions.Value,
        PermissionsStoreConstants.LorescopeWrite,
        PermissionsStoreConstants.LorescopeDelete
    ]);

    public static partial string InfiniloreAdmin { get; }
    public static Lazy<string[]> InfiniloreAdminPermissions { get; } = new(() => [
        ..ProducerPermissions.Value
    ]);

    public static partial string InfiniloreDeveloper { get; }
    public static Lazy<string[]> InfiniloreDeveloperPermissions { get; } = new(() => [
        ..InfiniloreAdminPermissions.Value
    ]);
}
