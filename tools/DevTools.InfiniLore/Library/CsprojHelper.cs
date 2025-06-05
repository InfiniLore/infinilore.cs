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
    public async Task ModifyProjectFileAsync(string projectPath, Action<XDocument> modificationAction, CancellationToken ct = default) {
        const int maxRetries = 3;
        const int delayMs = 1000;

        for (int i = 0; i < maxRetries; i++) {
            try {
                await using var fileStream = new FileStream(projectPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                
                XDocument doc = await XDocument.LoadAsync(fileStream, LoadOptions.None, ct);
                modificationAction(doc);
                
                fileStream.SetLength(0); // Clear existing content
                fileStream.Seek(0, SeekOrigin.Begin);
                await doc.SaveAsync(fileStream, SaveOptions.None, ct);
                return;
            }
            catch (IOException) when (i < maxRetries - 1) {
                logger.Warning("File {ProjectPath} is locked, waiting {Delay}ms before retry {Attempt}", 
                    projectPath, delayMs, i + 1);
                await Task.Delay(delayMs, ct);
            }
            catch (Exception ex) {
                logger.Error(ex, "Failed to modify project file: {ProjectPath}", projectPath);
                throw;
            }
        }
        
        throw new IOException($"Could not access file {projectPath} after {maxRetries} attempts");
    }

    public Task SetPropertyAsync(string projectPath, string propertyName, string propertyValue, CancellationToken ct = default) {
        return ModifyProjectFileAsync(projectPath, doc => {
            var propertyGroups = doc.Descendants("PropertyGroup");
            var firstPropertyGroup = propertyGroups.First();
            
            // Remove existing property if it exists
            firstPropertyGroup.Descendants(propertyName).Remove();
            
            // Add the new property
            firstPropertyGroup.Add(new XElement(propertyName, propertyValue));
        }, ct);
    }
}
