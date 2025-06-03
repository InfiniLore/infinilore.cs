// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<UserProfilesMapper>]
public class UserProfilesMapper : ResponseMapper<UserProfilesResponse, PaginatedData<InfiniLoreUserModel>> {
    public override UserProfilesResponse FromEntity(PaginatedData<InfiniLoreUserModel> entities) {
        var singleMapper = Resolve<UserProfileMapper>();

        UserProfileResponse[] items = entities.Items.Select(singleMapper.FromEntity).ToArray();
        return new UserProfilesResponse {
            Items = items,
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage
        };
    }
}