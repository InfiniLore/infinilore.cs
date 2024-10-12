// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.API.Dto;
using Microsoft.AspNetCore.Mvc;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetSpecificLoreScope;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetSpecificLoreScopeRequest : IRequiresUserId {
    [FromRoute] public Guid UserId { get; set; }
    [FromRoute] public Guid LoreScopeId { get; set; }
}
