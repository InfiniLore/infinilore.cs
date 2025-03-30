// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<ISingleLineSectionParser>("code", ServiceLifetime.Singleton)]
public class CodeInlineSectionParser : ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.Code;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin) {
        if (!entireMatch.Groups["codeText"].TryGetValue(out string? codeValue)) return;

        string normalizedBackticks = codeValue.Replace("\\`", "`");
        string output = HtmlEncoder.Default.Encode(normalizedBackticks);

        writer.Write("<code>");
        writer.Write(output);
        writer.Write("</code>");
    }
}
