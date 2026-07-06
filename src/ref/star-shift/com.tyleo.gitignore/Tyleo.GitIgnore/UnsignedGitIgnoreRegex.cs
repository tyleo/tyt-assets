#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tyleo.GitIgnore
{
    /// <summary>
    /// A specialized regular expression that matches file paths using
    /// glob-style patterns such as <c>**/HelloWorld.txt</c> or
    /// <c>/**/Temp/*</c>, similar to <c>.gitignore</c> rules.
    /// </summary>
    public readonly struct UnsignedGitIgnoreRegex
    {
        private static readonly Regex EmptyRegex =
            new(@"^$", RegexOptions.Compiled);

        private const string DirectorySeparatorCharacterPattern =
            @"[\\\/]";

        private const string StarRegexPattern =
            @"[\w !#\$%&'\(\)\+,\-.;=@\[\]\^`\{\}~]*";

        private const string DoubleStarRegexPattern =
            @"[\w !#\$%&'\(\)\+,\-.;=@\[\]\^`\{\}~\\\/]*";

        private const string DoubleStarWithTrailingSlashRegexPattern =
            @"([\w !#\$%&'\(\)\+,\-.;=@\[\]\^`\{\}~\\\/]*[\\\/])?";

        private readonly Regex _regex;

        /// <summary>
        /// Whether this pattern matches directories only or both
        /// files and directories.
        /// </summary>
        public readonly GitIgnoreRegexKind Kind;

        private UnsignedGitIgnoreRegex(Regex regex, GitIgnoreRegexKind kind)
        {
            _regex = regex;
            Kind = kind;
        }

        /// <summary>
        /// Compiles a gitignore glob pattern into a regex.
        /// </summary>
        /// <param name="span">The glob pattern to compile.</param>
        /// <returns>The compiled regex.</returns>
        public static UnsignedGitIgnoreRegex FromSpan(
            ReadOnlySpan<char> span
        )
        {
            if (span.Length == 0)
            {
                // An empty pattern matches nothing, so we return a regex that
                // matches the empty string.
                return new(
                    EmptyRegex,
                    GitIgnoreRegexKind.FileOrDirectory
                );
            }

            if (span.Length == 1)
            {
                switch (span[0])
                {
                    case '/':
                        // A single slash matches only directories, but it also
                        // only matches the empty string.
                        return new(
                            EmptyRegex,
                            GitIgnoreRegexKind.Directory
                        );

                    case '*':
                        span = "*".AsSpan();
                        break;
                }
            }

            span = ProcessPrefix(span, out var hasRootPrefix);
            span = ProcessSuffix(span, out var kind);

            var hasDirectorySeparator =
                hasRootPrefix ||
                span.IndexOf(Path.DirectorySeparatorChar) != -1 ||
                span.IndexOf(Path.AltDirectorySeparatorChar) != -1;

            var hasDoubleStar = false;
            for (var i = 0; i < span.Length;)
            {
                if (span[i] != '*')
                {
                    i++;
                    continue;
                }

                var starStart = i;
                while (i < span.Length && span[i] == '*')
                {
                    i++;
                }

                var starCount = i - starStart;
                if (starCount != 2) continue;

                var afterStartOrSlash =
                    starStart == 0 ||
                    span[starStart - 1].IsDirectorySeparatorChar();

                var beforeEndOrSlash =
                    i == span.Length ||
                    span[i].IsDirectorySeparatorChar();

                if (afterStartOrSlash && beforeEndOrSlash)
                {
                    hasDoubleStar = true;
                    break;
                }
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append('^');

            // If we don't have these things, we can detect the file in any
            // directory.
            if (!hasDirectorySeparator && !hasDoubleStar)
            {
                stringBuilder.Append(DoubleStarWithTrailingSlashRegexPattern);
            }

            for (var i = 0; i < span.Length; ++i)
            {
                if (span[i].IsDirectorySeparatorChar())
                {
                    // Escape
                    stringBuilder.Append(DirectorySeparatorCharacterPattern);
                }
                else if (span[i] == '*')
                {
                    var starStart = i;
                    while (i + 1 < span.Length &&
                        span[i + 1] == '*')
                    {
                        ++i;
                    }
                    var starCount = i - starStart + 1;

                    var isStandard = false;
                    if (starCount == 2)
                    {
                        var afterStartOrSlash =
                            starStart == 0 ||
                            span[starStart - 1].IsDirectorySeparatorChar();

                        var beforeEndOrSlash =
                            i + 1 == span.Length ||
                            span[i + 1].IsDirectorySeparatorChar();

                        isStandard = afterStartOrSlash && beforeEndOrSlash;
                    }

                    if (isStandard)
                    {
                        if (i + 1 < span.Length &&
                            span[i + 1]
                                .IsDirectorySeparatorChar())
                        {
                            ++i;
                            stringBuilder.Append(
                                DoubleStarWithTrailingSlashRegexPattern
                            );
                        }
                        else
                        {
                            stringBuilder.Append(
                                DoubleStarRegexPattern
                            );
                        }
                    }
                    else
                    {
                        stringBuilder.Append(StarRegexPattern);
                    }
                }
                else
                {
                    if (span[i].IsSpecialRegexChar()) stringBuilder.Append('\\');
                    stringBuilder.Append(span[i]);
                }
            }

            stringBuilder.Append('$');

            return new(
                new(stringBuilder.ToString(), RegexOptions.Compiled),
                kind
            );
        }

        /// <summary>
        /// Compiles a gitignore glob pattern, returning
        /// <c>null</c> for empty or comment lines.
        /// </summary>
        /// <param name="span">The glob pattern to compile.</param>
        /// <returns>
        /// The compiled regex, or <c>null</c> if the pattern is
        /// empty or a comment.
        /// </returns>
        public static UnsignedGitIgnoreRegex? FromSpanIgnoreInert(
            ReadOnlySpan<char> span
        )
        {
            if (span.Length == 0) return null;
            if (span[0] == '#') return null;
            return FromSpan(span);
        }

        /// <summary>
        /// Compiles multiple gitignore glob patterns into an array of regexes.
        /// </summary>
        /// <param name="spans">The glob patterns to compile.</param>
        /// <returns>An array of compiled unsigned regexes.</returns>
        public static UnsignedGitIgnoreRegex[] FromSpans(ReadOnlySpan<string> spans)
        {
            var result = new UnsignedGitIgnoreRegex[spans.Length];
            for (var i = 0; i < spans.Length; i++)
            {
                result[i] = FromSpan(spans[i]);
            }
            return result;
        }

        /// <summary>
        /// Compiles multiple gitignore glob patterns, returning an array of
        /// regexes, ignoring empty and comment lines.
        /// </summary>
        /// <param name="spans">The glob patterns to compile.</param>
        /// <returns>An array of compiled unsigned regexes.</returns>
        public static UnsignedGitIgnoreRegex[] FromSpansIgnoreInert(
            ReadOnlySpan<string> spans
        )
        {
            var list = new List<UnsignedGitIgnoreRegex>();
            for (var i = 0; i < spans.Length; i++)
            {
                var regex = FromSpanIgnoreInert(spans[i]);
                if (regex == null) continue;
                list.Add(regex.Value);
            }
            return list.ToArray();
        }

        private static ReadOnlySpan<char> ProcessPrefix(
            ReadOnlySpan<char> span,
            out bool hasRootPrefix
        )
        {
            if (span.Length > 0 && span[0].IsDirectorySeparatorChar())
            {
                hasRootPrefix = true;
                return span[1..];
            }
            hasRootPrefix = false;
            return span;
        }

        private static ReadOnlySpan<char> ProcessSuffix(
            ReadOnlySpan<char> span,
            out GitIgnoreRegexKind kind
        )
        {
            if (span.Length > 0 && span[^1].IsDirectorySeparatorChar())
            {
                kind = GitIgnoreRegexKind.Directory;
                return span[..^1];
            }

            kind = GitIgnoreRegexKind.FileOrDirectory;
            return span;
        }

        /// <summary>
        /// Determines whether the specified input matches the regex.
        /// </summary>
        /// <param name="input">The input string to match.</param>
        /// <returns>The result of the match.</returns>
        public readonly bool IsMatch(string input) => _regex.IsMatch(input);

        public readonly override string ToString() =>
            $"{{ \"Regex\": \"{_regex}\" , \"Kind\": \"{Kind}\" }}";
    }
}
