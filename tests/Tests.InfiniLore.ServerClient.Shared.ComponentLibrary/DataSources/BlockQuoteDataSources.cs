// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class BlockQuoteDataSources {
    private static readonly string SectionName = nameof(BlockQuoteDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > # test
            > > test
            >
            > - test
            > - test
            """,
            """
            <blockquote>
                <h1>test</h1>
                <blockquote>
                    <p>test</p>
                </blockquote>
                <ul>
                    <li>test</li>
                    <li>test</li>
                </ul>
            </blockquote>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > blockQuote 1
            >> ...blockQuote 2
            >>> ...blockQuote 3
            """,
            """
            <blockquote>
                <p>blockQuote 1</p>
                <blockquote>
                    <p>...blockQuote 2</p>
                    <blockquote>
                        <p>...blockQuote 3</p>
                    </blockquote>
                </blockquote>
            </blockquote>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > This is a blockquote with **bold text** and *italic text*.
            >
            > 1. First item
            > 2. Second item
            >    - Sub-item
            """,
            """
            <blockquote>
                <p>This is a blockquote with <strong>bold text</strong> and <em>italic text</em>.</p>
                <ol>
                    <li>First item</li>
                    <li>Second item
                        <ul>
                            <li>Sub-item</li>
                        </ul>
                    </li>
                </ol>
            </blockquote>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Nested quotes:
            > > Level 2
            > > > Level 3
            > > > > Level 4
            """,
            """
            <blockquote>
                <p>Nested quotes:</p>
                <blockquote>
                    <p>Level 2</p>
                    <blockquote>
                        <p>Level 3</p>
                        <blockquote>
                            <p>Level 4</p>
                        </blockquote>
                    </blockquote>
                </blockquote>
            </blockquote>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Combining elements:
            > ### Heading inside blockquote
            > - List item
            > - Another item
            > > Sub blockquote
            """,
            """
            <blockquote>
                <p>Combining elements:</p>
                <h3>Heading inside blockquote</h3>
                <ul>
                    <li>List item</li>
                    <li>Another item</li>
                </ul>
                <blockquote>
                    <p>Sub blockquote</p>
                </blockquote>
            </blockquote>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Inline formatting:
            > **Bold**, *italic*, `code`, and [link](#).
            """,
            """
            <blockquote>
                <p>Inline formatting:</p>
                <p><strong>Bold</strong>, <em>italic</em>, <code>code</code>, and <a href="#">link</a>.</p>
            </blockquote>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Mixed levels and lists:
            > 1. First
            >    > Nested blockquote with text.
            > 2. Second
            >    - Sub-item
            >       > Another nested quote.
            """,
            """
            <blockquote>
                <p>Mixed levels and lists:</p>
                <ol>
                    <li>
                        First
                        <blockquote>
                            <p>Nested blockquote with text.</p>
                        </blockquote>
                    </li>
                    <li>
                        Second
                        <ul>
                            <li>
                                Sub-item
                                <blockquote>
                                    <p>Another nested quote.</p>
                                </blockquote>
                            </li>
                        </ul>
                    </li>
                </ol>
            </blockquote>
            """
        );
    }
}
