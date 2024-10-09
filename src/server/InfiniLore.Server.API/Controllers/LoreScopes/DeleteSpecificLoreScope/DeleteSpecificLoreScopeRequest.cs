// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.API.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace InfiniLore.Server.API.Controllers.LoreScopes.DeleteSpecificLoreScope;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class DeleteSpecificLoreScopeRequest : IRequiresUserId {
    [FromRoute] public Guid UserId { get; set; }
    [FromRoute] public Guid LoreScopeId { get; set; }
}