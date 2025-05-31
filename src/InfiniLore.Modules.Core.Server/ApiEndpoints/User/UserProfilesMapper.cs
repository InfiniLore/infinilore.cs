// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.Database;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<UserProfilesMapper>]
public class UserProfilesMapper : AutoResponsesMapper<UserProfileResponse, UserProfileMapper, UserProfilesResponse, InfiniLoreUserModel>;