#nullable enable

using System;
using System.Collections.Generic;

namespace Tyleo.GitIgnore
{
    /// <summary>
    /// A specialized regular expression that matches file paths using
    /// glob-style patterns such as <c>**/HelloWorld.txt</c> or
    /// <c>/**/Temp/*</c>, similar to <c>.gitignore</c> rules.
    /// </summary>
    public readonly struct GitIgnoreRegex
    {
        /// <summary>
        /// The underlying compiled regex without sign information.
        /// </summary>
        public readonly UnsignedGitIgnoreRegex Unsigned;

        /// <summary>
        /// Whether this pattern includes (<c>true</c>) or excludes
        /// (<c>false</c>).
        /// </summary>
        public readonly bool Sign;

        private GitIgnoreRegex
        (
            UnsignedGitIgnoreRegex unsigned,
            bool sign
        )
        {
            Unsigned = unsigned;
            Sign = sign;
        }

        /// <summary>
        /// Compiles a gitignore glob pattern into a signed regex.
        /// A leading <c>!</c> negates the pattern.
        /// </summary>
        /// <param name="span">The glob pattern to compile.</param>
        /// <returns>The compiled signed regex.</returns>
        public static GitIgnoreRegex FromSpan(ReadOnlySpan<char> span)
        {
            var sign = true;
            if (span.StartsWith("!"))
            {
                sign = false;
                span = span[1..];
            }

            var unsigned = UnsignedGitIgnoreRegex.FromSpan(span);

            return new(unsigned, sign);
        }

        /// <summary>
        /// Compiles a gitignore glob pattern, returning
        /// <c>null</c> for empty or comment lines. A leading
        /// <c>!</c> negates the pattern.
        /// </summary>
        /// <param name="span">The glob pattern to compile.</param>
        /// <returns>
        /// The compiled signed regex, or <c>null</c> if the
        /// pattern is empty or a comment.
        /// </returns>
        public static GitIgnoreRegex? FromSpanIgnoreInert(
            ReadOnlySpan<char> span
        )
        {
            if (span.Length == 0) return default;
            if (span[0] == '#') return default;
            return FromSpan(span);
        }

        /// <summary>
        /// Compiles multiple gitignore glob patterns into an array of signed
        /// regexes.
        /// </summary>
        /// <param name="spans">The glob patterns to compile.</param>
        /// <returns>An array of compiled signed regexes.</returns>
        public static GitIgnoreRegex[] FromSpans(ReadOnlySpan<string> spans)
        {
            var result = new GitIgnoreRegex[spans.Length];
            for (var i = 0; i < spans.Length; i++)
            {
                result[i] = FromSpan(spans[i]);
            }
            return result;
        }

        /// <summary>
        /// Compiles multiple gitignore glob patterns, returning an array of
        /// signed regexes. Empty and comment lines are ignored.
        /// </summary>
        /// <param name="spans">The glob patterns to compile.</param>
        /// <returns>An array of compiled signed regexes.</returns>
        public static GitIgnoreRegex[] FromSpansIgnoreInert(
            ReadOnlySpan<string> spans
        )
        {
            var list = new List<GitIgnoreRegex>();
            for (var i = 0; i < spans.Length; i++)
            {
                var regex = FromSpanIgnoreInert(spans[i]);
                if (regex == null) continue;
                list.Add(regex.Value);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Aggregates the match result with the previous match result.
        /// </summary>
        /// <param name="path">The path to match.</param>
        /// <returns>
        /// The sign if the path is matched; <c>null</c> otherwise.
        /// </returns>
        public readonly bool? IsMatch(string path) =>
            Unsigned.IsMatch(path) ? Sign : null;

        public readonly override string ToString() =>
            $"{{ \"Unsigned\": {Unsigned} , \"Sign\": {Sign} }}";
    }
}
