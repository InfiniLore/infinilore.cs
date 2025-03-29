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
[KeyedInjectableService<IMultiLineSectionParser>("escaped", ServiceLifetime.Singleton)]
public class EscapedSectionParser : ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin => SingleLineOrigin.NotSkipped;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void ParseToStringBuilder(Match _, Group group, StringBuilder builder, SingleLineOrigin origin) {
       if (!group.TryGetValueSpan(out ReadOnlySpan<char> escapedCharSpan)) return;
       builder.Append(escapedCharSpan[1]);
    }
}
