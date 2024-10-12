// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.API.Dto;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetAll;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetAllLoreScopesRequest : IRequiresUserId {
    [FromRoute] public Guid UserId { get; set; }
}
