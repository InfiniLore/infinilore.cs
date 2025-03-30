// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<ISingleLineSectionParser>("linkRegular", ServiceLifetime.Singleton)]
public class LinkRegularSectionParser(IServiceProvider provider) : ISingleLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.Link;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin) {
        if (!entireMatch.Groups["lrText"].TryGetValue(out string? linkText)) return;
        if (!entireMatch.Groups["lrHref"].TryGetValue(out string? linkHref)) return;

        string titleText = entireMatch.Groups["lrTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

        if (entireMatch.Groups["lrBang"].Success) {
            writer.Write("<img src=\"");
            writer.Write(linkHref);
            writer.Write("\" alt=\"");
            writer.Write(linkText);
            writer.Write('"');
            writer.Write(titleText);
            writer.Write('>');
            return;
        }

        writer.Write("<a href=\"");
        writer.Write(linkHref);
        writer.Write("\">");
        _markdownParser.Value.ParseSingleline(linkText, writer, origin | SkipOnOrigin);
        writer.Write("</a>");
    }
}
