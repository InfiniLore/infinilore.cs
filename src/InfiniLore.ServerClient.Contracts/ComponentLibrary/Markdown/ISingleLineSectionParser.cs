// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin { get; }

    public void ParseToStringBuilder(Match entireMatch, Group group, IMarkdownWriter writer, SingleLineOrigin origin);
}
