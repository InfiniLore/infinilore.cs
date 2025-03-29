// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("listUnordered", ServiceLifetime.Singleton)]
public class ListUnorderedSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder) {
        if (!group.TryGetValue(out string? listUnorderedBody)) return;
        
        builder.Append("<ul>");
        foreach (Match lineMatch in MarkdownRegexLib.ListItemBodyRegex.Matches(listUnorderedBody)) {
            builder.Append("<li>");
            if (lineMatch.Groups["lHead"].TryGetValue(out string? listHeader)) {
                _markdownParser.Value.ParseSingleline(listHeader, builder);
            }

            if (lineMatch.Groups["lBody"].TryGetValue(out string? listBody)) {
                string normalizedBody = NormalizationHelper.NormalizeIndentation(listBody);
                _markdownParser.Value.ParseMultiline(normalizedBody, builder);
            }

            builder.Append("</li>");
        }

        builder.Append("</ul>");
    }
}
