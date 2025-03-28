// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HorizontalLineDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {
        var chars = new[] { '-', '*', '_' };
        foreach (char c in chars) {
            for (int i = 1; i < 10; i++) {
                string text = new (c, i);
                string content = i < 3 
                    ? $"<p>{text}</p>" 
                    : "<hr>";
                yield return () => new MultilineDataDto(text, content);
            }
        }
    }
    
}
