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
[KeyedInjectableService<IMultiLineSectionParser>("horizontalRule", ServiceLifetime.Singleton)]
public class HorizontalRuleSectionParser : IMultiLineSectionParser {
    public void ParseToStringBuilder(Match _, Group group, StringBuilder builder) {
        builder.Append("<hr>");
    }
}
