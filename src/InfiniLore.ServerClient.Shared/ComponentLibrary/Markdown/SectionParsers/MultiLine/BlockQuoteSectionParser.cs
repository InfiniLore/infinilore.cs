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
[KeyedInjectableService<IMultiLineSectionParser>("blockQuote", ServiceLifetime.Singleton)]
public class BlockQuoteSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match _, Group group, StringBuilder builder) {
        if(!group.TryGetValue(out string? blockQuoteBody)) return;
        
        string normalized = MarkdownRegexLib.NormalizeBlockQuoteRegex.Replace(blockQuoteBody, string.Empty);
        string adjustedBlockquote = NormalizationHelper.NormalizeIndentation(normalized);
        
        builder.Append("<blockquote>");
        provider.GetRequiredService<IMarkdownParser>().ParseMultiline(adjustedBlockquote, builder);
        builder.Append("</blockquote>");
    }
}
