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
            Markdown: """
            1. this
            2. is
            3. a
              - nested
            4. list
            """,
            HtmlOutput: """
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
