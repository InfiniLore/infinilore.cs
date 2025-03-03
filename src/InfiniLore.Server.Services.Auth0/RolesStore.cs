// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;

namespace InfiniLore.Server.Services.Auth0;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RolesStore]
public partial class RolesStore {
    public static partial string User { get; }
    public static string[] UserPermissions { get; } = [
        PermissionsStore.LorescopeRead
    ];
    
    public static partial string Consumer { get; }
    public static string[] ConsumerPermissions { get; } = [
        ..UserPermissions,
        PermissionsStore.LorescopeWrite,
        PermissionsStore.LorescopeDelete
    ];
    
    public static partial string Producer { get; }
    public static string[] ProducerPermissions { get; } = [
        ..ConsumerPermissions
    ];
    
    public static partial string InfiniloreAdmin { get; }
    public static string[] InfiniloreAdminPermissions { get; } = [
        ..ProducerPermissions
    ];
    
    public static partial string InfiniloreDeveloper { get; }
    public static string[] InfiniloreDeveloperPermissions { get; } = [
        ..InfiniloreAdminPermissions
    ];
}
