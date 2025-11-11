// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;

namespace InfiniLore.Core.Modules.Users.Api;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("user-create")]
public partial class CreateUserCliCommand : ICliCommand<CreateUserCliParameters>{

    public ValueTask ExecuteAsync(CreateUserCliParameters parameters, CancellationToken ct = new CancellationToken()) {
        
        throw new NotImplementedException();
    }
}
