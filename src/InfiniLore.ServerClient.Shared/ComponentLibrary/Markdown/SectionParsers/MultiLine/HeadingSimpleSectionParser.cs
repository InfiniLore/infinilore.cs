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
[KeyedInjectableService<IMultiLineSectionParser>("headingSimple", ServiceLifetime.Singleton)]
public class HeadingSimpleSectionParser(IServiceProvider provider) : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder) {
        if(!entireMatch.Groups["hsText"].TryGetValue(out string? headerSimpleText)) return;
        
        builder.Append("<h1>");
        provider.GetRequiredService<IMarkdownParser>().ParseSingleline(headerSimpleText, builder);
        builder.Append("</h1>");
    }
}
