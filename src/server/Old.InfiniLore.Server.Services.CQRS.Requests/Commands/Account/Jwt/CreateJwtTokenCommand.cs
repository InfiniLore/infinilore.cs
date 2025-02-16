// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Contracts.Services.CQRS;
using Old.InfiniLore.Server.Types;

namespace Old.InfiniLore.Server.Services.CQRS.Requests.Commands.Account.Jwt;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct CreateJwtTokenCommand(
    InfiniLoreUser User,
    string[] Roles,
    string[] Permissions,
    int? RefreshExpiresInDays = null
) : ICqrsRequest<JwtTokenData>;
