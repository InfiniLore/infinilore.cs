// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("codeBlock", ServiceLifetime.Singleton)]
public class CodeBlockSectionParser : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        if (!entireMatch.Groups["cBody"].TryGetValue(out string? codeBlockBody)) return;

        string langName = entireMatch.Groups["cLang"].TryGetValue(out string? langNameValue)
            ? langNameValue
            : string.Empty;

        string langClass = langName.IsNotNullOrWhiteSpace()
            ? $" class=\"language-{langName}\""
            : string.Empty;

        writer.Write("<pre><code")
            .Write(langClass)
            .Write('>')
            .Write(HtmlEncoder.Default.Encode(codeBlockBody))
            .Write("</code></pre>");
    }
}
