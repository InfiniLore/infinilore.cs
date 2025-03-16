// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Credentials;

namespace InfiniLore.Server.Services.Auth0;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CredentialsStore(CredentialsFlags.PermissionsStore, true)]
public static partial class PermissionsStore {
    private const string Account = nameof(Account);
    private const string Data = nameof(Data);
    private const string System = nameof(System);
    private const string User = nameof(User);

    private const string DataUser = nameof(DataUser);
    private const string DataSystem = nameof(DataSystem);

    public static partial string AccountRead { get; }
    public static partial string AccountWrite { get; }
    public static partial string AccountDelete { get; }

    [Prefix(Data)] public static partial string SystemRead { get; }
    [Prefix(Data)] public static partial string SystemWrite { get; }

    [Prefix(DataUser)] public static partial string LorescopeRead { get; }
    [Prefix(DataUser)] public static partial string LorescopeWrite { get; }
    [Prefix(DataUser)] public static partial string LorescopeDelete { get; }

    [Prefix(DataUser)] public static partial string ProfileRead { get; }
}
