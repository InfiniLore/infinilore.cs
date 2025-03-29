// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TableDataSources {
    private static readonly string SectionName = nameof(TableDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            Markdown: """
            | Column 1      | Column 2     | Column 3      |
            | --------------| ------------ |-------------- |
            | Row 2 col 1   | Row 2 col 2  | Row 2 col 3   |
            """,

            HtmlOutput: """
            <table>
                <thead>
                    <tr>
                        <th>Column 1</th>
                        <th>Column 2</th>
                        <th>Column 3</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Row 2 col 1</td>
                        <td>Row 2 col 2</td>
                        <td>Row 2 col 3</td>
                    </tr>
                </tbody>
            </table>
            """
        );
    }
}
