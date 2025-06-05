// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace DevTools.InfiniLore.Library;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<TemplateHelper>]
public class TemplateHelper {
    public async Task<string> LoadTemplateModuleEntryFileAsync(string moduleName, string mode, CancellationToken ct = default) {
        string templatePath = Path.Combine("Commands", "ModuleSetup", "Templates", "TemplateIModuleEntry.txt");
        string template = await LoadTemplateAsync(templatePath, ct);
        return template.Replace("{ModuleName}", moduleName).Replace("{Mode}", mode);
    }
    
    public async Task<string> LoadTemplateModuleSetupFileAsync(string moduleName, string mode, CancellationToken ct = default) {
        string templatePath = Path.Combine("Commands", "ModuleSetup", "Templates", "TemplateModuleSetup.txt");
        string template = await LoadTemplateAsync(templatePath, ct);
        return template.Replace("{ModuleName}", moduleName).Replace("{Mode}", mode);
    }
    
    private static async Task<string> LoadTemplateAsync(string relativePath, CancellationToken ct = default) {
        if (!File.Exists(relativePath)) throw new FileNotFoundException($"Template file not found: {relativePath}");
        return await File.ReadAllTextAsync(relativePath, ct);
    }
}
