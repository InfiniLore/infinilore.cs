// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Data.User;
using Old.InfiniLore.Contracts.Services.CQRS;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace Old.InfiniLore.Server.Services.CQRS.Requests.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record DeleteLorescopeCommand(
    HttpContext HttpContext,
    Guid LorescopeId
) : ICqrsRequest<LorescopeModel>;
