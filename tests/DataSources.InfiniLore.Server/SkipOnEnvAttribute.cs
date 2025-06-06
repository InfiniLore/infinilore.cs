// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace DataSources.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SkipOnEnvAttribute(string envVar) : SkipAttribute($"Test skipped because environment variable '{envVar}' is set") {
    private static readonly string[] TrueValues = { "1", "true", "yes", "on" };
    
    public override Task<bool> ShouldSkip(BeforeTestContext context) {
        string? envValue = Environment.GetEnvironmentVariable(envVar);

        if (envValue.IsNullOrEmpty()) return Task.FromResult(false);
        return Task.FromResult(TrueValues.Contains(envValue.ToLowerInvariant()));
    }

}
