// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.MultiLine;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[KeyedInjectableService<IMultiLineSectionParser>("blockQuote", ServiceLifetime.Singleton)]
public class BlockQuoteSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match _, Group group, IMarkdownWriter writer) {
        if(!group.TryGetValue(out string? blockQuoteBody)) return;
        
        string normalized = MarkdownRegexLib.NormalizeBlockQuoteRegex.Replace(blockQuoteBody, string.Empty);
        string adjustedBlockquote = NormalizationHelper.NormalizeIndentation(normalized);
        
        writer.Write("<blockquote>");
        _markdownParser.Value.ParseMultiline(adjustedBlockquote, writer);
        writer.Write("</blockquote>");
    }
}
