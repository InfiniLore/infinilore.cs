// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.MultilineDataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ListsDataSources {
    public static IEnumerable<Func<MultilineDataDto>> Data() {

        yield return static () => new MultilineDataDto(
            """
            1. this
            2. is
            3. a
              - nested
            4. list
            """,
            """
            <ol>
                <li>this</li>
                <li>is</li>
                <li>a
                    <ul>
                    <li>nested</li>
                    </ul>
                </li>
                <li>list</li>
            </ol>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            - Item 1
              - Subitem 1.1
              - Subitem 1.2
            - Item 2
            """,
            """
            <ul>
                <li>Item 1
                    <ul>
                        <li>Subitem 1.1</li>
                        <li>Subitem 1.2</li>
                    </ul>
                </li>
                <li>Item 2</li>
            </ul>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            1. Ordered item 1
            2. Ordered item 2
               1. Nested ordered item 2.1
               2. Nested ordered item 2.2
            3. Ordered item 3
            """,
            """
            <ol>
                <li>Ordered item 1</li>
                <li>Ordered item 2
                    <ol>
                        <li>Nested ordered item 2.1</li>
                        <li>Nested ordered item 2.2</li>
                    </ol>
                </li>
                <li>Ordered item 3</li>
            </ol>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            - Unordered item 1
            - Unordered item 2
              1. Mixed nested ordered item 2.1
              2. Mixed nested ordered item 2.2
            - Unordered item 3
            """,
            """
            <ul>
                <li>Unordered item 1</li>
                <li>Unordered item 2
                    <ol>
                        <li>Mixed nested ordered item 2.1</li>
                        <li>Mixed nested ordered item 2.2</li>
                    </ol>
                </li>
                <li>Unordered item 3</li>
            </ul>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            1. First ordered item
               - Subitem A
               - Subitem B
            2. Second ordered item
            """,
            """
            <ol>
                <li>First ordered item
                    <ul>
                        <li>Subitem A</li>
                        <li>Subitem B</li>
                    </ul>
                </li>
                <li>Second ordered item</li>
            </ol>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            - Top-level item
                - Subitem level 1
                    - Subitem level 2
                        - Subitem level 3
            """,
            """
            <ul>
                <li>Top-level item
                    <ul>
                        <li>Subitem level 1
                            <ul>
                                <li>Subitem level 2
                                    <ul>
                                        <li>Subitem level 3</li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            - Top item 1
            - Top item 2
            
                Unrelated text block in the same list

            - Top item 3
            """,
            """
            <ul>
                <li>Top item 1</li>
                <li>Top item 2</li>
            </ul>
            <p></p>
            <p>Unrelated text block in the same list</p>
            <p></p>
            <ul>
                <li>Top item 3</li>
            </ul>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            1.    Extra spaces for alignment
                  - This is a sublist
            2.    Another item
            """,
            """
            <ol>
                <li>Extra spaces for alignment
                    <ul>
                        <li>This is a sublist</li>
                    </ul>
                </li>
                <li>Another item</li>
            </ol>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            - A list item with **bold text**
            - A list item with *italic text*
            - A list item with `inline code`
            """,
            """
            <ul>
                <li>A list item with <strong>bold text</strong></li>
                <li>A list item with <em>italic text</em></li>
                <li>A list item with <code>inline code</code></li>
            </ul>
            """
        );

        yield return static () => new MultilineDataDto(
            """
            - This list has
              - Uneven indentation
                - But still renders
                  - Correctly!
            """,
            """
            <ul>
                <li>This list has
                    <ul>
                        <li>Uneven indentation
                            <ul>
                                <li>But still renders
                                    <ul>
                                        <li>Correctly!</li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
            """
        );


        // yield return static () => new MultilineDataDto(
        //     Markdown: """
        //     1. this
        //     2. [ ] is
        //     3. [x] a
        //       - [X] nested
        //     4. todo list
        //     """,
        //     HtmlOutput: """
        //     <ol>
        //         <li>this</li>
        //         <li class="task"><input type="checkbox">is</li>
        //         <li class="task"><input type="checkbox" checked="">a
        //             <ul>
        //             <li class="task">
        //             <input type="checkbox" disabled="" checked="">nested</li>
        //             </ul>
        //         </li>
        //         <li>todo list</li>
        //     </ol>
        //     """
        // );
    }
}
