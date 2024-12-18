// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Server.Contracts.Services.CQRS;
using InfiniLore.Server.Contracts.Types;

// ReSharper disable once CheckNamespace
namespace InfiniLore.Server.Services.CQRS.Requests.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetOneLorescopeQuery(
    UserIdUnion UserIdUnion,
    Guid LorescopeId
) : ICqrsRequest<LorescopeModel>;
