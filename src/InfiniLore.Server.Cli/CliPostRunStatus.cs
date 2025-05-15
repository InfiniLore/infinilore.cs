// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Services;

namespace InfiniLore.Server.Cli;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<ICliPostRunStatus>]
public class CliPostRunStatus : ICliPostRunStatus {
    public bool ShouldExit { get; set; } = false;
}
