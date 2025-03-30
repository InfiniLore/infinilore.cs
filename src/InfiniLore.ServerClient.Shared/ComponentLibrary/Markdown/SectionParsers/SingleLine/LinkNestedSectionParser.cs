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
[KeyedInjectableService<ISingleLineSectionParser>("linkNested", ServiceLifetime.Singleton)]
public class LinkNestedSectionParser(IServiceProvider provider) : ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.NotSkipped;
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin) {
        if (!entireMatch.Groups["lnText"].TryGetValue(out string? linkText)) return;
        if (!entireMatch.Groups["lnHref"].TryGetValue(out string? linkHref)) return;
        string titleText = entireMatch.Groups["lnTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

        if (entireMatch.Groups["lnBang"].Success) {
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
        
        _markdownParser.Value.ParseSingleline(linkText, writer, origin);
        writer.Write("</a>");
    }
}
