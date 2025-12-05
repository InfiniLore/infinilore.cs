// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Messaging;
using InfiniLore.Modules.Users.Database;

namespace InfiniLore.Modules.Users.Messaging.Queries;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetUserQuery : BaseQuery<UserModel> {
    public Guid UserId { get; private set; } = Guid.Empty;
    public string? Username { get; private set; }
    
    public static GetUserQuery FromUserId(Guid userId, QueryConfig config = QueryConfig.None) => new() {
        UserId = userId,
        Config = config
    };
    public static GetUserQuery FromUsername(string username, QueryConfig config = QueryConfig.None) => new() {
        Username = username, 
        Config = config
    };
}
