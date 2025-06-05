// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace DevTools.InfiniLore.Library;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<CsprojHelper>]
public class CsprojHelper(ILogger<CsprojHelper> logger) {
    private async Task ModifyProjectFileAsync(string projectPath, Action<XDocument> modificationAction, CancellationToken ct = default) {
        try {
            await using FileStream fileStream = File.OpenRead(projectPath);
            XDocument doc = await XDocument.LoadAsync(fileStream, LoadOptions.None, ct);
            modificationAction(doc);
            
            await using FileStream writeStream = File.Create(projectPath);
            await doc.SaveAsync(writeStream, SaveOptions.None, ct);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to modify project file: {ProjectPath}", projectPath);
            throw;
        }
    }

    public Task SetPropertyAsync(string projectPath, string propertyName, string propertyValue, CancellationToken ct = default) {
        return ModifyProjectFileAsync(projectPath, doc => {
            IEnumerable<XElement> propertyGroups = doc.Descendants("PropertyGroup");
            XElement firstPropertyGroup = propertyGroups.First();
            
            // Remove existing property if it exists
            firstPropertyGroup.Descendants(propertyName).Remove();
            
            // Add the new property
            firstPropertyGroup.Add(new XElement(propertyName, propertyValue));
        }, ct);
    }


}
