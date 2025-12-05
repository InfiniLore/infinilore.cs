// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;

namespace InfiniLore.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserModelRepository : IBaseModelRepository<UserModel> {
    ValueTask<UserModel?> GetByUserNameAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default);
    ValueTask<bool> IsUserNameTakenAsync(string username, CancellationToken ct = default);
}
