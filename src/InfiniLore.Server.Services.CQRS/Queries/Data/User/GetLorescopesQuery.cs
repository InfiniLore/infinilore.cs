// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Services.CQRS.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLorescopesQuery(
    Guid UserId,
    bool AutoInclude = false,
    PaginationInfo PaginationInfo = default,
    bool Reverse = false
) : MediatorRequest<PaginatedResult<LoreScope>>;
