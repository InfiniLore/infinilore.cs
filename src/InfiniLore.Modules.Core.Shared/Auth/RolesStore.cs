// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace InfiniLore.Shared.Auth;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CredentialsStore(CredentialsFlags.IterateValues, true, ".")]
public static partial class RolesStore {

    public static Lazy<FrozenDictionary<string, ImmutableArray<string>>> PermissionsPerRoles { get; } = new(PermissionsPerRolesFactory);
    private static FrozenDictionary<string, ImmutableArray<string>> PermissionsPerRolesFactory() {
        return new Dictionary<string, ImmutableArray<string>> {
            [User] = UserPermissionsFactory(),
            [Consumer] = UserPermissionsFactory(),
            [Producer] = UserPermissionsFactory(),
            [InfiniloreAdmin] = UserPermissionsFactory(),
            [InfiniloreDeveloper] = UserPermissionsFactory()
        }.ToFrozenDictionary();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Assignments
    // -----------------------------------------------------------------------------------------------------------------
    public static partial string User { get; }
    public static Lazy<ImmutableArray<string>> UserPermissions { get; } = new(UserPermissionsFactory);
    private static ImmutableArray<string> UserPermissionsFactory() {
        return [
            PermissionsStoreConstants.LorescopeRead,
        ];
    }

    public static partial string Consumer { get; }
    public static Lazy<ImmutableArray<string>> ConsumerPermissions { get; } = new(ConsumerPermissionsFactory);
    private static ImmutableArray<string> ConsumerPermissionsFactory() {
        return [
            ..UserPermissionsFactory()
        ];
    }

    public static partial string Producer { get; }
    public static Lazy<ImmutableArray<string>> ProducerPermissions { get; } = new(ProducerPermissionsFactory);
    private static ImmutableArray<string> ProducerPermissionsFactory() {
        return [
            ..ConsumerPermissionsFactory(),
            PermissionsStoreConstants.LorescopeWrite,
            PermissionsStoreConstants.LorescopeDelete,
            PermissionsStoreConstants.LorescopePosterWrite,
        ];
    }

    public static partial string InfiniloreAdmin { get; }
    public static Lazy<ImmutableArray<string>> InfiniloreAdminPermissions { get; } = new(InfiniLoreAdminPermissionsFactory);
    private static ImmutableArray<string> InfiniLoreAdminPermissionsFactory() {
        return [
            ..ProducerPermissionsFactory()
        ];
    }

    public static partial string InfiniloreDeveloper { get; }
    public static Lazy<ImmutableArray<string>> InfiniloreDeveloperPermissions { get; } = new(() => [
        ..InfiniLoreAdminPermissionsFactory()
    ]);
}
