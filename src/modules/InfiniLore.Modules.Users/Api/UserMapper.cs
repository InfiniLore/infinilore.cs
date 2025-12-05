// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Modules.Users.Database;

namespace InfiniLore.Modules.Users.Api;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<UserMapper>]
public class UserMapper : ResponseMapper<UserResponse, UserModel> {
    public override UserResponse FromEntity(UserModel model) => new() {
        UserId = model.Id,
        UserName = model.UserName
    };

    public override Task<UserResponse> FromEntityAsync(UserModel e, CancellationToken ct) 
        => Task.FromResult(FromEntity(e));
}
