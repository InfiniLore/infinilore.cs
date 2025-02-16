// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Permissions;

namespace Old.InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// [InjectableService<ApiPermissions>(ServiceLifetime.Singleton)] // Having this as a singleton could be a good idea in the future, but for now it's not needed.
[PermissionsStore(GeneratorFlags.ParsePrefix | GeneratorFlags.GenerateAllPermissionsMethod)]
public static partial class ApiPermissionsStore {
    // -----------------------------------------------------------------------------------------------------------------
    // SectionNames
    // -----------------------------------------------------------------------------------------------------------------
    private const string Data = nameof(Data);
    private const string User = nameof(User);
    private const string System = nameof(System);
    private const string Discovery = nameof(Discovery);

    private const string Account = nameof(Account);
    // -----------------------------------------------------------------------------------------------------------------
    // Permissions
    // -----------------------------------------------------------------------------------------------------------------
    [Prefix(Data, User)] public static partial string LorescopeRead { get; }// can read data from user's lorescopes
    [Prefix(Data, User)] public static partial string LorescopeWrite { get; }// can write data to user's lorescopes
    [Prefix(Data, User)] public static partial string LorescopeDelete { get; }// can delete  user's lorescopes
    [Prefix(Data, User)] public static partial string LorescopeManage { get; }// can manage  user's lorescopes
    [Prefix(Data, Discovery)] public static partial string LorescopeDiscover { get; }// Can look up lorescopes
    [Prefix(Data, System)] public static partial string LorescopesInfo { get; }// can read system info about lorescopes
}
