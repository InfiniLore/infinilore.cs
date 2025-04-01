// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.ServerClient.ComponentLibrary.Markdown;
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.MarkdownWriters;
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.Pools;
using InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown.SectionParsers.SingleLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IMarkdownParser>(ServiceLifetime.Singleton)]
public class MarkdownParser(IServiceProvider serviceProvider, ILogger<MarkdownParser> logger) : IMarkdownParser {
    private readonly FrozenDictionary<string, IMultiLineSectionParser> MultilineGroupToParsers = ToFrozenDictionary<IMultiLineSectionParser>(MultilineGroupNames, logger, serviceProvider);
    private readonly FrozenDictionary<string, ISingleLineSectionParser> SinglelineGroupToParsers = ToFrozenDictionary<ISingleLineSectionParser>(SinglelineGroupNames, logger, serviceProvider);

    private static ImmutableArray<string> MultilineGroupNames => [
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

    private static ImmutableArray<string> SinglelineGroupNames => [
        "escaped",
        "boldAndItalic",
        "bold",
        "italic",
        "supScript",
        "subScript",
        "strike",
        "code",
        "linkNested",
        "linkRegular",
        "underline",
        "emote",
        "tag",
        // "remainder" // Remainder for single-lines are their own separate thing, see service below
    ];
    private readonly RemainderSectionParser RemainderSectionParser = (RemainderSectionParser)serviceProvider.GetRequiredKeyedService<ISingleLineSectionParser>("remainder");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string Parse(string markdown) {
        StringBuilderMarkdownWriter writer = StringBuilderMarkdownWriterPool.Get();
        try {
            ParseMultiline(markdown, writer);
            return writer.ToString();
        }
        finally {
            StringBuilderMarkdownWriterPool.Return(writer);
        }
    }

    public void Parse<T>(string markdown, T writer) where T : TextWriter {
        var markdownWriter = new TextWriterMarkdownWriter<T>(writer);
        ParseMultiline(markdown, markdownWriter);
    }

    private static FrozenDictionary<string, T> ToFrozenDictionary<T>(ImmutableArray<string> keyNames, ILogger<MarkdownParser> logger, IServiceProvider serviceProvider) {
        int keyCount = keyNames.Length;
        var dictionaryBuilder = new Dictionary<string, T>(keyCount);

        for (int index = 0; index < keyCount; index++) {
            string groupName = keyNames[index];
            var service = serviceProvider.GetKeyedService<T>(groupName);
            if (service is null) {
                logger.LogWarning($"No service found for group name '{groupName}' for type '{typeof(T).Name}'.");
                continue;
            }

            dictionaryBuilder[groupName] = service;
        }

        return dictionaryBuilder.ToFrozenDictionary();

    }

    #region Parsing Methods
    public void ParseMultiline(string markdown, IMarkdownWriter writer) {
        Queue<Match> matchesQueue = MatchQueuePool.Get();

        try {
            // Preload matches into the queue
            MatchCollection matches = MarkdownRegexLib.MultilineStructuresMatches(markdown);
            matchesQueue.EnsureCapacity(matches.Count);
            foreach (Match match in matches) {
                matchesQueue.Enqueue(match);
            }

            // Process matches
            while (matchesQueue.TryDequeue(out Match? match)) {
                GroupCollection groups = match.Groups;
                int count = groups.Count;

                for (int index = 0; index < count; index++) {
                    Group group = groups[index];
                    if (!group.Success) continue;
                    if (!MultilineGroupToParsers.TryGetValue(group.Name, out IMultiLineSectionParser? sectionParser)) continue;

                    sectionParser.ParseToStringBuilder(match, group, writer);
                }
            }
        }
        finally {
            matchesQueue.Clear();
            MatchQueuePool.Return(matchesQueue);
        }

    }


    public void ParseSingleline(string markdown, IMarkdownWriter writer, SingleLineOrigin origin = SingleLineOrigin.Undefined) {
        Queue<Match> matchesQueue = MatchQueuePool.Get();

        try {
            // Preload matches into the queue
            MatchCollection matches = MarkdownRegexLib.SinglelineStructuresMatches(markdown);
            matchesQueue.EnsureCapacity(matches.Count);
            foreach (Match match in matches) {
                matchesQueue.Enqueue(match);
            }

            // Track the current index in the markdown span
            int currentIndex = 0;
            ReadOnlySpan<char> markdownSpan = markdown.AsSpan();

            // Process all matches
            while (matchesQueue.TryDequeue(out Match? match)) {
                GroupCollection groups = match.Groups;
                int count = groups.Count;
                int matchIndex = match.Index;

                // Add unmatched text before the current match
                if (matchIndex > currentIndex) {
                    ReadOnlySpan<char> unmatchedText = markdownSpan.Slice(currentIndex, matchIndex - currentIndex);
                    RemainderSectionParser.ParseToStringBuilder(ref unmatchedText, writer);
                }

                for (int index = 0; index < count; index++) {
                    Group group = groups[index];
                    if (!group.Success) continue;
                    if (!SinglelineGroupToParsers.TryGetValue(group.Name, out ISingleLineSectionParser? sectionParser)) continue;
                    if (origin.HasFlag(sectionParser.SkipOnOrigin)) continue;

                    sectionParser.ParseToStringBuilder(match, group, writer, origin);
                }

                // Update the current position to the end of the current match
                currentIndex = matchIndex + match.Length;
            }

            if (currentIndex < markdown.Length) {
                ReadOnlySpan<char> remainingText = markdownSpan[currentIndex..];
                RemainderSectionParser.ParseToStringBuilder(ref remainingText, writer);
            }
        }
        finally {
            matchesQueue.Clear();
            MatchQueuePool.Return(matchesQueue);
        }
    }
    #endregion
}
