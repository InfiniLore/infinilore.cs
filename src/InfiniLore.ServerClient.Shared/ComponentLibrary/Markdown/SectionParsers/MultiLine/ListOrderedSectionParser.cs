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
[KeyedInjectableService<IMultiLineSectionParser>("listOrdered", ServiceLifetime.Singleton)]
public class ListOrderedSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder) {
        if (!group.TryGetValue(out string? listOrderedBody)) return;
        
        builder.Append("<ol>");
        foreach (Match lineMatch in MarkdownRegexLib.ListItemBodyRegex.Matches(listOrderedBody)) {
            builder.Append("<li>");

            if (lineMatch.Groups["lHead"].TryGetValue(out string? listHeader)) {
                provider.GetRequiredService<IMarkdownParser>().ParseSingleline(listHeader, builder);
            }

            if (lineMatch.Groups["lBody"].TryGetValue(out string? listBody)) {
                string normalizedBody = NormalizationHelper.NormalizeIndentation(listBody);
                provider.GetRequiredService<IMarkdownParser>().ParseMultiline(normalizedBody, builder);
            }

            builder.Append("</li>");
        }

        builder.Append("</ol>");
    }
}
