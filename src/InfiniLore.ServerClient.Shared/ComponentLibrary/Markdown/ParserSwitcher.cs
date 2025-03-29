// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using System.Text;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ParserSwitcher(IServiceProvider serviceProvider) : IParserSwitcher {
    private FrozenDictionary<string, IMultiLineSectionParser> MultilineGroupToParserDictionary { get;  } = MultilineGroupNames
        .Select(groupName => (GroupName:groupName, Service:serviceProvider.GetKeyedService<IMultiLineSectionParser>(groupName)))
        .Where(tuple => tuple.Service is not null)
        .ToFrozenDictionary(tuple => tuple.GroupName, tuple => tuple.Service!);
    
    private FrozenDictionary<string, ISingleLineSectionParser> SinglelineGroupToParserDictionary { get;  } = SinglelineGroupNames
        .Select(groupName => (GroupName:groupName, Service:serviceProvider.GetKeyedService<ISingleLineSectionParser>(groupName)))
        .Where(tuple => tuple.Service is not null)
        .ToFrozenDictionary(tuple => tuple.GroupName, tuple => tuple.Service!);
    
    private static readonly string[] MultilineGroupNames = [
        "remainder",
        "heading",
        "codeBlock",
        "headingSimple",
        "listUnordered",
        "listOrdered",
        "table",
        "blockQuote",
        "htmlBody",
        "horizontalRule"
    ];
    
    private static readonly string[] SinglelineGroupNames = [
        "escaped",
        "boldAndItalic"
    ];
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string ParseMultiline(string markdown) {
        StringBuilder builder = StringBuilderPool.Get();
        try {
            ParseMultiline(markdown, builder);
            return builder.ToString();
        }
        finally {
            StringBuilderPool.Return(builder);
        }
    }

    public void ParseMultiline(string markdown, StringBuilder builder) {
        MatchCollection collection = MarkdownRegexLib.MultilineStructuresMatches(markdown);
        for (int index = 0; index < collection.Count; index++) {
            Match match = collection[index];
            foreach ((string groupName, IMultiLineSectionParser sectionParser) in MultilineGroupToParserDictionary) {
                if (match.Groups[groupName] is not {Success: true} group) continue;
                sectionParser.ParseToStringBuilder(match, group, builder);
            }
        }
    }

    public string ParseSingleline(string markdown) {
        StringBuilder builder = StringBuilderPool.Get();
        try {
            ParseSingleline(markdown, builder);
            return builder.ToString();
        }
        finally {
            StringBuilderPool.Return(builder);
        }
    }
    
    public void ParseSingleline(string markdown, StringBuilder builder, SingleLineOrigin origin = SingleLineOrigin.Undefined) {
        MatchCollection collection = MarkdownRegexLib.SinglelineStructuresMatches(markdown);
        for (int index = 0; index < collection.Count; index++) {
            Match match = collection[index];
            foreach ((string groupName, ISingleLineSectionParser sectionParser) in SinglelineGroupToParserDictionary) {
                if (origin.HasFlag(sectionParser.SkipOnOrigin)) continue;
                if (match.Groups[groupName] is not {Success: true} group) continue;
                sectionParser.ParseToStringBuilder(match, group, builder, origin);
            }
        }
    }
}
