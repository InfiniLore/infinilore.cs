// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.API.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoreScopeResponse(
    [UsedImplicitly] Guid Id,
    [UsedImplicitly] string UserId,
    [UsedImplicitly] string Name,
    [UsedImplicitly] string Description,
    [UsedImplicitly] ICollection<Guid> MultiverseIds
);
