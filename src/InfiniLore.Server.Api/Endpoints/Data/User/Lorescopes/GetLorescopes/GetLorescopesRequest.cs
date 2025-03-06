// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts;
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Endpoints.Data.User.Lorescopes.GetLorescopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public record GetLorescopesRequest(
    Guid UserId,
    [property: FromBody] PaginationInfo PaginationInfo = default,
    [property: FromBody] bool Reverse = false
);
