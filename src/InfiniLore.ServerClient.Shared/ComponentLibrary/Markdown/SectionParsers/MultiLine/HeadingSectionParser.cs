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
[KeyedInjectableService<IMultiLineSectionParser>("heading", ServiceLifetime.Singleton)]
public class HeadingSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    private readonly Lazy<IMarkdownParser> _markdownParser = new(provider.GetRequiredService<IMarkdownParser>);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder) {
        if(!entireMatch.Groups["hLevel"].TryGetLength(out int headingLevel)) return;
        if(!entireMatch.Groups["hText"].TryGetValue(out string? headerText)) return;
        
        builder.Append("<h").Append(headingLevel).Append('>');
        _markdownParser.Value.ParseSingleline(headerText, builder);
        builder.Append("</h").Append(headingLevel).Append('>');
    }
}
