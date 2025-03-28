// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class HtmlDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {
        yield return static () => new MultilineDataDto(
            Markdown: """
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
            HtmlOutput: """
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

        yield return () => new MultilineDataDto(
            Markdown: """
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
            HtmlOutput: """
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
    }
}
