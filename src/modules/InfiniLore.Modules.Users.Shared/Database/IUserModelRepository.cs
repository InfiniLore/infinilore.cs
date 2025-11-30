// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;

namespace InfiniLore.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserModelRepository : IBaseModelRepository<UserModel> {
    ValueTask<RepoOutcome<UserModel>> GetByUserNameAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default);
    ValueTask<bool> IsUserNameTakenAsync(string username, CancellationToken ct = default);
}
