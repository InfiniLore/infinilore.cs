// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Core.Pagination;
using InfiniLore.Modules.Users.Database;

namespace InfiniLore.Modules.Users.Api;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<UsersMapper>]
public class UsersMapper : ResponseMapper<UsersResponse, PaginatedData<UserModel>> {
    public override UsersResponse FromEntity(PaginatedData<UserModel> data) {
        var userMapper = Resolve<UserMapper>();
        
        return new UsersResponse {
            Users = data.Items.Select(userMapper.FromEntity).ToArray(),
            TotalCount = data.TotalCount,
            CurrentPage = data.CurrentPage,
        };
    }

    public override Task<UsersResponse> FromEntityAsync(PaginatedData<UserModel> e, CancellationToken ct) 
        => Task.FromResult(FromEntity(e));
}
