// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Data.User;
using Old.InfiniLore.Contracts.Services.CQRS;

// ReSharper disable once CheckNamespace
namespace Old.InfiniLore.Server.Services.CQRS.Requests.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record struct CreateLorescopeCommand(
    LorescopeModel Lorescope
) : ICqrsRequest<LorescopeModel>;
