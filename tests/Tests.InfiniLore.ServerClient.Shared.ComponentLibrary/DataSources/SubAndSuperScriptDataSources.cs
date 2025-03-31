// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace Tests.InfiniLore.ServerClient.Shared.ComponentLibrary.DataSources;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class SubAndSuperScriptDataSources {
    private static readonly string SectionName = nameof(SubAndSuperScriptDataSources)[..^nameof(DataSources).Length];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IEnumerable<Func<MarkdownTestDto>> DataSources() {
        yield return static () => new MarkdownTestDto(SectionName,
            "^^superscript^^",
            "<p><sup>superscript</sup></p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "^^\\^superscript^^",
            "<p><sup>^superscript</sup></p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "^^superscript\\^^^",
            "<p><sup>superscript^</sup></p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "^subscript^",
            "<p><sub>subscript</sub></p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "^subscript\\^^",
            "<p><sub>subscript^</sub></p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "^\\^subscript^",
            "<p><sub>^subscript</sub></p>"
        );
        
        yield return static () => new MarkdownTestDto(SectionName,
            "This is a **bold^^superscript^^ text**.",
            "<p>This is a <strong>bold<sup>superscript</sup> text</strong>.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "Text with *italic^subscript^ and ^^superscript^^*.",
            "<p>Text with <em>italic<sub>subscript</sub> and <sup>superscript</sup></em>.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "A [link with ^^superscript^^ and ^subscript^](https://example.com).",
            """
            <p>A <a href="https://example.com">link with <sup>superscript</sup> and <sub>subscript</sub></a>.</p>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            - **Bold^^sup^^**
            - *Italic^sub^*
            """,
            """
            <ul>
                <li><strong>Bold<sup>sup</sup></strong></li>
                <li><em>Italic<sub>sub</sub></em></li>
            </ul>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "Nested formatting: **Bold ^subscript^ and ^^superscript^^ in a [link](https://example.com)**.",
            """
            <p>Nested formatting: <strong>Bold <sub>subscript</sub> and <sup>superscript</sup> in a <a href="https://example.com">link</a></strong>.</p>
            """
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "Inline code with superscript and subscript: `x = y^^2^^ - z^2^`",
            "<p>Inline code with superscript and subscript: <code>x = y^^2^^ - z^2^</code></p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            "Complex: ***Bold and italic^^super^^ and italic^sub^***.",
            "<p>Complex: <strong><em>Bold and italic<sup>super</sup> and italic<sub>sub</sub></em></strong>.</p>"
        );

        yield return static () => new MarkdownTestDto(SectionName,
            """
            - **Bold link [with ^^superscript^^ text](https://example.com)**.
            - *Italic link [and subscript ^sub^ text](https://example.org)*.
            """,
            """
            <ul>
                <li><strong>Bold link <a href="https://example.com">with <sup>superscript</sup> text</a></strong>.</li>
                <li><em>Italic link <a href="https://example.org">and subscript <sub>sub</sub> text</a></em>.</li>
            </ul>
            """
        );

    }
}
