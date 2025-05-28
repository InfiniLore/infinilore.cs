// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public record UpsertLoreScopeImageRequest(
    Guid UserId,
    Guid LoreScopeId,
    string FileName,
    string ContentType,
    IFormFile File
);
