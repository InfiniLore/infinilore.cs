// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using FastEndpoints;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Messaging.Commands;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Users.Cli;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("user-create")]
public partial class CreateUserCliCommand(ILogger<CreateUserCliCommand> logger) : ICliCommand<CreateUserCliParameters>{
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(CreateUserCliParameters parameters, CancellationToken ct = new()) {
        var command = new CreateUserCommand(parameters.Username);
        Outcome<Guid> outcome = await command.ExecuteAsync(ct: ct);
        
        outcome.Switch(
            guid => logger.Information("User created with id: {id}", guid),
            error => logger.Error("Failed to create user with error: {error}", error)
        );
    }
}
