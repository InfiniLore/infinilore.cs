// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Contracts.Services.CQRS;

namespace Old.InfiniLore.Server.Services.CQRS.Requests.Commands.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoginCommand(
    string Username,
    string Password
) : ICqrsRequest<InfiniLoreUser>;
