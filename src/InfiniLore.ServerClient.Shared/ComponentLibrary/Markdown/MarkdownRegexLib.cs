// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace InfiniLore.ServerClient.Shared.ComponentLibrary.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class MarkdownRegexLib {
    [GeneratedRegex("""
          (?<escaped>\\[][!"\#$%&'()*+,\-./:;<=>?@\\^_`{|}~])
        | (?<bold>\*\*(?<b>(?>[^\\\*]+|\\\*|\*|(?<open>\*\*)|(?<-open>\*\*))+?\*?)(?(open)(?!))\*\*)
        | (?<italic>\*(?<i>(?>[^\\\*]+|\\\*|\*\*|(?<open>\*)|(?<-open>\*))+)(?(open)(?!))\*)
        | (?<supScript>\^\^(?<sp>(?>[^\\\^]+|\\\^|\^|(?<open>\^\^)|(?<-open>\^\^))+?\^?)(?(open)(?!))\^\^)
        | (?<subScript>\^(?<sb>(?>[^\\\^]+|\\\^|\^\^|(?<open>\^)|(?<-open>\^))+)(?(open)(?!))\^)
        | (?<strike>~(?<s>.+?)(?<!\\)~)
        | (?<underline>_(?<u>.+?)(?<!\\)_)
        | (?<code>(?<open>`+)(?<c>(?>[^`\\]+|\\.|`(?!\k<open>))*?)\k<open>)
        | (?<emote>:(?<e>[a-zA-Z0-9-_]+):)
        | (?<linkNested>
            (?<lnBang>!)?
            \[(?<lnText>!?\[.+?\]\(.+?\))\]
            \((?<lnHref>.+?)(?:\s?"(?<lnTitle>[^"]*)")?\)
          )
        | (?<linkRegular>
            (?<lrBang>!)?
            \[(?<lrText>[^\]]+?)\]
            \((?<lrHref>[^\)]+?)(?:\s?"(?<lrTitle>[^"]*)")?\)
          )
        | (?<tag>\#(?<tText>[^#\s]+))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex SinglelineStructuresRegex { get; }

    [GeneratedRegex("""
          (?<heading>^(?<hLevel>\#{1,6})\s+(?<hText>.+))
        | (?<codeBlock>`{3}(?<cLang>.*?)?\r?\n(?<cBody>[\s\S]*?)`{3})
        | (?<headingSimple>^(?<hsText>.+?)\r?\n[\ ]*[-=]{3,})
        | (?<listUnordered>(?:^[^\S\r\n]*-\s+.*(?:\n(?:[^\S\r\n]*[-.]\d*\.?\s+.*|[^\S\r\n]+.*))*))
        | (?<listOrdered>(?:^[^\S\r\n]*[-.]?\d+\.?\s+.*(?:\n(?:[^\S\r\n]*[-.]?\d+\.?\s+.*|[^\S\r\n]+.*))*))
        | (?<table>
            ^\|(?<tHead>.+)\|\s*\r?\n
            ^\|(?<tSep>[:\-|\ ]+?)\|\s*\r?\n
            (?<tBody>(?:^\|.*\|\s*)+)
          )
        | (?<blockQuote>^>\s+.*(?:\r?\n(?![*+-]\s|[-.]?\d|\s*[^>]).+)*)
        | (?:
            (?<htmlPre>.+?)?
            (?<htmlBody>
              <(?<tag>\w+)\b[^:>]*>
              (?>
                [^<]+
                | <(?<open>\k<tag>)\b[^>]*>
                | </(?<-open>\k<tag>)>
                | <(?!/?\k<tag>\b)[^>]+>
              )*
              (?(open)(?!))
              </\k<tag>>
            )
            (?<htmlPost>.+)?
          )
        | (?<horizontalRule>^[*-_]{3,}\s*$)
        | (?<remainder>.+?(?:\r?\n|$))
        """, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled)]
    private static partial Regex MultilineStructuresRegex { get; }

    [GeneratedRegex(@"^[ ]*[-.]?\d*\.?\s+(?<lTask>\[[\ xX]\])?(?<lHead>.+)(?<lBody>(?:\n[ ]+.+)*)", RegexOptions.Multiline | RegexOptions.ExplicitCapture)]
    public static partial Regex ListItemBodyRegex { get; }

    [GeneratedRegex("^>", RegexOptions.Multiline)]
    public static partial Regex NormalizeBlockQuoteRegex { get; }
    
    [GeneratedRegex("\r?\n")]
    public static partial Regex NormalizeNewlinesRegex { get; }
    
    [GeneratedRegex(@"<[sS][cC][rR][iI][pP][tT].*?>[\s\S]*?</[sS][cC][rR][iI][pP][tT]>")]
    public static partial Regex NormalizeScriptRegex { get; }

    public static MatchCollection SinglelineStructuresMatches(string markdown) => SinglelineStructuresRegex.Matches(markdown);
    public static MatchCollection MultilineStructuresMatches(string markdown) => MultilineStructuresRegex.Matches(markdown);
}
