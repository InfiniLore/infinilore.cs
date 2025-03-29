// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.ComponentLibrary.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMultiLineSectionParser {
    public void ParseToStringBuilder(Match entireMatch, Group group, StringBuilder builder);
}
