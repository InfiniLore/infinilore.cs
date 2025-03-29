// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<ISingleLineSectionParser>("linkRegular", ServiceLifetime.Singleton)]
public class LinkRegularSectionParser(IServiceProvider provider) : ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.Link;
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder, SingleLineOrigin origin) {
        if (!entireMatch.Groups["lrText"].TryGetValue(out string? linkText)) return;
        if (!entireMatch.Groups["lrHref"].TryGetValue(out string? linkHref)) return;
        string titleText = entireMatch.Groups["lrTitle"].TryGetValue(out string? altTextValue) ? $" title=\"{altTextValue}\"" : string.Empty;

        if (entireMatch.Groups["lrBang"].Success) {
            builder.Append("<img src=\"");
            builder.Append(linkHref);
            builder.Append("\" alt=\"");
            builder.Append(linkText);
            builder.Append('"');
            builder.Append(titleText);
            builder.Append('>');
            return;
        }
        
        builder.Append("<a href=\"");
        builder.Append(linkHref);
        builder.Append("\">");
        _markdownParser.Value.ParseSingleline(linkText, builder, origin | SkipOnOrigin);
        builder.Append("</a>");
    }
}
