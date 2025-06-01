// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using InfiniLore.Server.Cli.DataSeeding.Seeders;

namespace InfiniLore.Server.Cli.DataSeeding;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("seed-data")]
public partial class DataSeedingCommand(
    ILogger<DataSeedingCommand> logger,
    IServiceProvider serviceProvider,
    CliPostRunEffects cliPostRunEffects
) : ICliCommand<DataSeedingParameters> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(DataSeedingParameters parameters, CancellationToken ct = default) {
        if (parameters.Section.IsNullOrWhiteSpace()) {
            IEnumerable<ISeeder> sections = serviceProvider.GetServices<ISeeder>();
            await Task.WhenAll(sections.Select(section => section.StartSeedingAsync(ct)));
            return;
        }

        var section = serviceProvider.GetKeyedService<ISeeder>(parameters.Section);
        if (section is null) {
            logger.Error("Could not find a seeder service by their key-name {key}", parameters.Section);
            cliPostRunEffects.ExitOnCompletion();
            return;
        }

        await section.StartSeedingAsync(ct);
    }
}
