// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Contracts.Services.CQRS;

namespace InfiniLore.Server.Services.CQRS.Requests.Commands.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LoginCommand(
    string Username,
    string Password
) : ICqrsRequest<InfiniLoreUser>;
