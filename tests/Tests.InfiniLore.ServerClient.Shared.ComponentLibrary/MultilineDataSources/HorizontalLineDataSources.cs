// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HorizontalLineDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {
        for (int i = 0; i < 10; i++) {
            string text = new ('-', i);
            string content = i < 3 
                ? $"<p>{text}</p>" 
                : "<hr>";
            yield return () => new MultilineDataDto(text, content);
        }
    }
    
}
