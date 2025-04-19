// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetLoreScopesQuery(
    Guid UserId,
    bool AutoInclude = false,
    PaginationInfo PaginationInfo = default,
    bool Reverse = false
) : MessageRequest<PaginatedData<LoreScope>>;
