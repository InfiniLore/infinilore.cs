// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Services.Mediator.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLoreScopesQuery(
    Guid UserId,
    bool AutoInclude = false,
    PaginationInfo PaginationInfo = default,
    bool Reverse = false
) : MediatorRequest<PaginatedData<LoreScope>>;
