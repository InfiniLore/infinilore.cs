// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Contracts.Services.CQRS;
using InfiniLore.Server.Types;

namespace InfiniLore.Server.Services.CQRS.Requests.Commands.Account.Jwt;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct CreateJwtTokenCommand(
    InfiniLoreUser User,
    string[] Roles,
    string[] Permissions,
    int? RefreshExpiresInDays = null
) : ICqrsRequest<JwtTokenData>;
