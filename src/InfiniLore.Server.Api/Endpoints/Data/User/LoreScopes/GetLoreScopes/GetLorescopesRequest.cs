// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts;
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Endpoints.Data.User.LoreScopes.GetLoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public record GetLoreScopesRequest(
    Guid UserId
    // PaginationInfo PaginationInfo = default,
    // bool Reverse = false
);
