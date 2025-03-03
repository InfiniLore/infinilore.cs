// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;

namespace InfiniLore.Server.Services.Auth0;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[PermissionsStore]
public static partial class PermissionsStore {
    private const string Account = "Account";
    private const string Data = "Data";
    private const string System = "System";
    private const string User = "User";
    
    private const string SystemData = Data+System;
    [Prefix(SystemData)] public static partial string SystemRead { get; }
    
    private const string UserData = Data+User;
    [Prefix(UserData)] public static partial string LorescopeRead { get; }
    [Prefix(UserData)] public static partial string LorescopeWrite { get; }
    [Prefix(UserData)] public static partial string LorescopeDelete { get; }
}
