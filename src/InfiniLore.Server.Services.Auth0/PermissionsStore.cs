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
    
    private const string DataSystem = Data+System;
    [Prefix(DataSystem)] public static partial string SystemRead { get; }
    
    private const string DataUser = Data+User;
    [Prefix(DataUser)] public static partial string LorescopeRead { get; }
    [Prefix(DataUser)] public static partial string LorescopeWrite { get; }
    [Prefix(DataUser)] public static partial string LorescopeDelete { get; }
}
