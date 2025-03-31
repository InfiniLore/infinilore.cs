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
[KeyedInjectableService<IMultiLineSectionParser>("htmlBody", ServiceLifetime.Singleton)]
public class HtmlBodySectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer) {
        if (!entireMatch.Groups["htmlPre"].TryGetValue(out string? pre)) {
            writer.Write(SanitizeHtml(group.Value));
            return;
        }

        writer.Write("<p>");
        _markdownParser.Value.ParseSingleline(pre, writer);
        writer.Write(SanitizeHtml(group.Value));
        if (entireMatch.Groups["htmlPost"].TryGetValue(out string? post)) {
            _markdownParser.Value.ParseSingleline(post, writer);
        }
        writer.Write("</p>");
    }

    private static string SanitizeHtml(string html) {
        // TODO look into actual html sanitizing libraries
        string sanitized = MarkdownRegexLib.NormalizeScriptRegex.Replace(html, static match => HtmlEncoder.Default.Encode(match.Value));
        return sanitized;
    }
}
