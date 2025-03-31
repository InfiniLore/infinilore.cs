// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AggregateDataSources {
    private static readonly string SectionName = nameof(AggregateDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {

        yield return static () => new MarkdownTestDto(SectionName,
            """
            ## Longer Example with Multiple Sections

            ### Introduction
            Welcome to this test. This section introduces the topic, along with **bold**, *italic*, and `code` formatting.

            ### Code Snippet
            Below is an example of a C# code snippet:

            ```csharp
            public class Program {
                public static void Main() {
                    Console.WriteLine("Hello, World!");
                }
            }
            ```

            ### Bullet Points
            Here are some bullet points:
            - Point one
            - Point two
              - Subpoint A
              - Subpoint B

            ### Blockquote
            > This is a blockquote. It can contain multiple lines of text
            > and demonstrates how Markdown handles quoted content.


            ### Table Example
            | Column 1       | Column 2       | Column 3       |
            |----------------|----------------|----------------|
            | Data 1         | Data 2         | Data 3         |
            | Data 4         | Data 5         | Data 6         |

            ### Links and Images
            You can visit [Google](https://www.google.com) or check out the following image:

            ![Placeholder Image](https://via.placeholder.com/150)
            """,
            """
            <h2>Longer Example with Multiple Sections</h2>
            <h3>Introduction</h3>
            <p>Welcome to this test. This section introduces the topic, along with <strong>bold</strong>, <em>italic</em>, and <code>code</code> formatting.</p>
            <h3>Code Snippet</h3>
            <p>Below is an example of a C# code snippet:</p>
            <pre><code class="language-csharp">public class Program {&#xD;&#xA;
                public static void Main() {&#xD;&#xA;
                    Console.WriteLine(&quot;Hello, World!&quot;);&#xD;&#xA;
                }&#xD;&#xA;
            }&#xD;&#xA;</code></pre>
            <h3>Bullet Points</h3>
            <p>Here are some bullet points:</p>
            <ul>
                <li>Point one</li>
                <li>Point two
                <ul>
                    <li>Subpoint A</li>
                    <li>Subpoint B</li>
                </ul>
                </li>
            </ul>
            <h3>Blockquote</h3>
            <blockquote>
                <p>This is a blockquote. It can contain multiple lines of text</p>
                <p>and demonstrates how Markdown handles quoted content.</p>
            </blockquote>
            <h3>Table Example</h3>
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
                        <td>Data 1</td>
                        <td>Data 2</td>
                        <td>Data 3</td>
                    </tr>
                    <tr>
                        <td>Data 4</td>
                        <td>Data 5</td>
                        <td>Data 6</td>
                    </tr>
                </tbody>
            </table>
            <h3>Links and Images</h3>
            <p>You can visit <a href="https://www.google.com">Google</a> or check out the following image:</p>
            <p><img src="https://via.placeholder.com/150" alt="Placeholder Image"></p>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            ## Nested Lists and Complex Formatting

            - **Main Topic 1**
              - Subtopic 1.1
                - Detail 1.1.1
                - Detail 1.1.2
                  - Extra Detail 1.1.2.1
              - Subtopic 1.2
            - **Main Topic 2**
              1. Item 2.1
              2. Item 2.2
                 - Sub-Item 2.2.1
                 - Sub-Item 2.2.2
            """,
            """
            <h2>Nested Lists and Complex Formatting</h2>
            <ul>
                <li><strong>Main Topic 1</strong>
                <ul>
                    <li>Subtopic 1.1
                    <ul>
                        <li>Detail 1.1.1</li>
                        <li>Detail 1.1.2
                        <ul>
                            <li>Extra Detail 1.1.2.1</li>
                        </ul>
                        </li>
                    </ul>
                    </li>
                    <li>Subtopic 1.2</li>
                </ul>
                </li>
                <li><strong>Main Topic 2</strong>
                <ol>
                    <li>Item 2.1</li>
                    <li>Item 2.2
                    <ul>
                        <li>Sub-Item 2.2.1</li>
                        <li>Sub-Item 2.2.2</li>
                    </ul>
                    </li>
                </ol>
                </li>
            </ul>
            """
        );
    }
}
