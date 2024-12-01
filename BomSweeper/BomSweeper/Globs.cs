namespace BomSweeper;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Provides utilities to convert a glob pattern (path containing wildcard
/// characters) to the Regular Expression (RE) pattern.
/// </summary>
public static partial class Globs
{
    private const string DoubleAsteriskSlash = "**/";

    private static readonly Regex SlashSequencePattern
        = NewSlashSequencePattern();

    private static readonly ImmutableHashSet<char> NeedsEscapeCharSet = [
        '\\', '[', ']', '.', '*', '+', '?', '^', '$', '(', ')', '{', '}', '|'];

    /// <summary>
    /// Gets the RE string corresponding to the specified glob patterns.
    /// </summary>
    /// <param name="all">
    /// All the glob patterns. The path separator must be slash ('/').
    /// </param>
    /// <returns>
    /// The RE string corresponding to the specified glob patterns, which
    /// starts with '^' and ends with '$' for the single-line mode of <see
    /// cref="RegexOptions"/>.
    /// </returns>
    public static string ToPattern(IEnumerable<string> all)
    {
        return string.Join(
            string.Empty,
            all.Select(ToPattern)
                .Separate("|")
                .Prepend("^(")
                .Append(")$"));
    }

    private static string ToPattern(string rawInput)
    {
        static void EndsWithAny(StringBuilder b)
            => b.Append(".+");

        /*
            Case 1. just equals double asterisk
                |*|*|

            Case 2-a. starts with double asterisk and slash
                |*|*|/|...

            Case 2-b. ends with slash and double asterisk
                ...|/|*|*|

            Case 2-c. contains double asterisk between slashes (one or more times)
                ...|/|*|*|/|...
        */

        // Case 1
        if (rawInput is "**")
        {
            return ".+";
        }

        var input = SlashSequencePattern.Replace(rawInput, "/");
        var inputLength = input.Length;
        var b = new StringBuilder(inputLength);
        var k = 0;
        var (n, lastHook) = EndsWith(input, inputLength, "/**")
            ? // case 2-b
              (inputLength - 2, EndsWithAny)
            : (inputLength, new Action<StringBuilder>(b => {}));

        void SkipRepeating(string p)
        {
            var m = p.Length;
            while (StartsWith(input, k, n, p))
            {
                k += m;
            }
        }

        void SkipRepeatingChar(char c)
        {
            while (k < n && input[k] == c)
            {
                ++k;
            }
        }

        bool SkipIfStartsWith(string p)
        {
            if (!StartsWith(input, k, n, p))
            {
                return false;
            }
            k += p.Length;
            return true;
        }

        bool SkipIfStartsWithChar(char c)
        {
            if (input[k] == c)
            {
                ++k;
                return true;
            }
            return false;
        }

        // Case 2-a
        if (SkipIfStartsWith(DoubleAsteriskSlash))
        {
            b.Append("([^/]+/)*");
            SkipRepeating(DoubleAsteriskSlash);
        }

        while (k < n)
        {
            // Case 2-c
            // |0|1|2|3|
            // |/|*|*|/|
            if (SkipIfStartsWith("/**/"))
            {
                b.Append("/([^/]+/)*");
                SkipRepeating(DoubleAsteriskSlash);
                continue;
            }

            // |0|
            // |*|
            if (SkipIfStartsWithChar('*'))
            {
                b.Append("[^/]*");
                SkipRepeatingChar('*');
                continue;
            }

            var c = input[k];
            if (NeedsEscapeCharSet.Contains(c))
            {
                b.Append('\\');
            }
            ++k;
            b.Append(c);
        }

        lastHook(b);
        return b.ToString();
    }

    private static bool StartsWith(string s, int o, int n, string p)
    {
        var m = p.Length;
        return o + m <= n
            && string.CompareOrdinal(s, o, p, 0, m) == 0;
    }

    private static bool EndsWith(string s, int n, string p)
    {
        var m = p.Length;
        return n >= m
            && s.EndsWith(p, StringComparison.Ordinal);
    }

    [GeneratedRegex("//+")]
    private static partial Regex NewSlashSequencePattern();
}
