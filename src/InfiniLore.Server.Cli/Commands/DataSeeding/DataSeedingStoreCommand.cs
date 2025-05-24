// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Cli.DataSeeding;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("seed-store")]
public partial class DataSeedingStoreCommand(
    ILogger<DataSeedingStoreCommand> logger
) : ICliCommand<DataSeedingStoreParameters> {
    
    public ValueTask ExecuteAsync(DataSeedingStoreParameters parameters, CancellationToken ct = default) {
        logger.Critical("NOT YET IMPLEMENTED");
        throw new NotImplementedException();
    }
}
