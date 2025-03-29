// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HorizontalLineDataSources {
    private static readonly string SectionName = nameof(HorizontalLineDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        var chars = new[] { '-', '*', '_' };
        foreach (char c in chars) {
            for (int i = 1; i < 10; i++) {
                string text = new (c, i);
                string content = i < 3 
                    ? $"<p>{text}</p>" 
                    : "<hr>";
                yield return () => new MarkdownTestDto(SectionName,text, content);
            }
        }
    }
    
}
