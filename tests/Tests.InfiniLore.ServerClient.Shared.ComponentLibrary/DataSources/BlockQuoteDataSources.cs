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
        
        yield return static () => new MarkdownTestDto(SectionName,
            ">", 
            "<p>&gt;</p>" // an empty blockquote is not parsed as a blockquote
        );
        
        
        yield return static () => new MarkdownTestDto(SectionName,
            "> Plain text blockquote.",
            """
            <blockquote>
                <p>Plain text blockquote.</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > This is a blockquote with escaped characters: \*, \>, `\``.
            """,
            """
            <blockquote>
                <p>This is a blockquote with escaped characters: *, >, <code>`</code>.</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Level 1
            > > Level 2
            > Back to Level 1
            > > > Level 3
            """,
            """
            <blockquote>
                <p>Level 1</p>
                <blockquote>
                    <p>Level 2</p>
                </blockquote>
                <p>Back to Level 1</p>
                <blockquote>
                    <blockquote>
                        <p>Level 3</p>
                    </blockquote>
                </blockquote>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > **Bold text**, *italic text*, `inline code`, ~strikethrough~, and [link](#).
            """,
            """
            <blockquote>
                <p><strong>Bold text</strong>, <em>italic text</em>, <code>inline code</code>, <s>strikethrough</s>, and <a href="#">link</a>.</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Line 1
            >
            >
            > Line 2
            """,
            """
            <blockquote>
                <p>Line 1</p>
                <p>Line 2</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > This is a paragraph.
            >
            > ```
            > Code block inside a blockquote.
            > ```
            """,
            """
            <blockquote>
                <p>This is a paragraph.</p>
                <pre><code>Code block inside a blockquote.&#xA;</code></pre>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > This is the first paragraph.
            >
            > This is the second paragraph, separated by an empty line.
            """,
            """
            <blockquote>
                <p>This is the first paragraph.</p>
                <p>This is the second paragraph, separated by an empty line.</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > <div>This is a div inside a blockquote.</div>
            """,
            """
            <blockquote>
                <div>This is a div inside a blockquote.</div>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Level 1
            >>>>>>>>>>>>>>>>>>>> Level 20
            """,
            """
            <blockquote>
                <p>Level 1</p>
                <p>&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;&gt;Level 20</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > - First item
            >    - Nested item with **bold text**.
            > - Second item
            """,
            """
            <blockquote>
                <ul>
                    <li>First item
                        <ul>
                            <li>Nested item with <strong>bold text</strong>.</li>
                        </ul>
                    </li>
                    <li>Second item</li>
                </ul>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > Line 1
            >> Line 2
            >>> Line 3
            > Line 4
            """,
            """
            <blockquote>
                <p>Line 1</p>
                <blockquote>
                    <p>Line 2</p>
                    <blockquote>
                        <p>Line 3</p>
                    </blockquote>
                </blockquote>
                <p>Line 4</p>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            > This is a blockquote
            >
            > - List item 1
            > - List item 2
            """,
            """
            <blockquote>
                <p>This is a blockquote</p>
                <ul>
                    <li>List item 1</li>
                    <li>List item 2</li>
                </ul>
            </blockquote>
            """
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            """
            - List item
                > This is a blockquote inside a list item.
            """,
            """
            <ul>
                <li>
                    List item
                    <blockquote>
                        <p>This is a blockquote inside a list item.</p>
                    </blockquote>
                </li>
            </ul>
            """
        );
    }
}
