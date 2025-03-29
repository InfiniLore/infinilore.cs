// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.ComponentLibrary.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ISingleLineSectionParser {
    public SingleLineOrigin SkipOnOrigin { get; }
    
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder, SingleLineOrigin origin);
}
