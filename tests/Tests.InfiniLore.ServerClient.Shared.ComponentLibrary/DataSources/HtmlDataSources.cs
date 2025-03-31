// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HtmlDataSources {
    private static readonly string SectionName = nameof(HtmlDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            """
            Unrelated previous paragraph followed by a blank line
            <table>
            <tr>
            <td>Table cell</td>
            <td>

            <table>
            <tr>
            <td>*Tables in tables*</td>
            </tr>
            </table>

            </td>
            </tr>
            </table>
            """,
            """
            <p>Unrelated previous paragraph followed by a blank line</p>
            <table>
            <tr>
            <td>Table cell</td>
            <td>

            <table>
            <tr>
              <td>*Tables in tables*</td>
            </tr>
            </table>

            </td>
            </tr>
            </table>
            """
        );

        yield return () => new MarkdownTestDto(SectionName,
            """
            <pre>
            Buffalo Bill ’s
            defunct
                   who used to
                   ride a watersmooth-silver
                                            stallion
            and break onetwothreefourfive pigeonsjustlikethat
                                                             Jesus
            he was a handsome man
                                 and what i want to know is
            how do you like your blueeyed boy
            Mister Death
            </pre>
            """,
            """
            <pre>
            Buffalo Bill ’s
            defunct
                   who used to
                   ride a watersmooth-silver
                                            stallion
            and break onetwothreefourfive pigeonsjustlikethat
                                                             Jesus
            he was a handsome man
                                 and what i want to know is
            how do you like your blueeyed boy
            Mister Death
            </pre>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            test <div>
            <bold>something</bold>
            </div>
            """,
            """
            <p> test <div>
            <bold>something</bold>
            </div> </p>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            test <div>
            <script>something</script>
            </div>
            """,
            """
            <p> test <div>
            &lt;script&gt;something&lt;/script&gt;
            </div></p> 
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "test <div> <script>something</script> </div>",
            """
            <p> test <div>
            &lt;script&gt;something&lt;/script&gt;
            </div> </p> 
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "test <script>something</script>",
            "<p> test &lt;script&gt;something&lt;/script&gt;</p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            *test* <div>
            <bold>something</bold>
            </div> **bold this**
            """,
            """
            <p> <em>test</em> <div>
            <bold>something</bold>
            </div> <strong>bold this</strong> </p>
            """
        );
    }
}
