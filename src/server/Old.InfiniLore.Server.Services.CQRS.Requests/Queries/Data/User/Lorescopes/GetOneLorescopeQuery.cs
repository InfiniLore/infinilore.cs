// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Data.User;
using Old.InfiniLore.Contracts.Services.CQRS;

// ReSharper disable once CheckNamespace
namespace Old.InfiniLore.Server.Services.CQRS.Requests.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct GetOneLorescopeQuery(
    Guid LorescopeId
) : ICqrsRequest<LorescopeModel>;
