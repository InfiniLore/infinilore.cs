// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Services;

namespace InfiniLore.Server.Cli;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<ICliPostRunEffects>]
public class CliPostRunEffects : ICliPostRunEffects {
    public bool ShouldExit { get; set; }
    public void ExitOnCompletion() => ShouldExit = true;
}
