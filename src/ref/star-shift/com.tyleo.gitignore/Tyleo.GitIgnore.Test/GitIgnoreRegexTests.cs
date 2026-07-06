#nullable enable

using NUnit.Framework;

namespace Tyleo.GitIgnore.Test
{
    internal static class GitIgnoreRegexTests
    {
        public record IsMatchTestData(
            string Pattern,
            string Test,
            bool Match,
            GitIgnoreRegexKind? Kind = null,
            bool Sign = true
        )
        {
            public override string ToString() =>
                $"{{ \"Pattern\": \"{Pattern}\", \"Test\": \"{Test}\", \"Match\": {Match}, \"Kind\": {(Kind == null ? "null" : $"\"{Kind}\"")}, \"Sign\": {Sign} }}";
        }

        private static IsMatchTestData[] IsMatchTestSource() => new[]
        {
            new IsMatchTestData("", "", true),
            new IsMatchTestData("", ";", false),
            new IsMatchTestData("!", "", true, Sign: false, Kind: GitIgnoreRegexKind.FileOrDirectory),
            new IsMatchTestData("!", ";", false, Sign: false, Kind: GitIgnoreRegexKind.FileOrDirectory),

            new IsMatchTestData(";", "", false),
            new IsMatchTestData(";", ";", true),

            new IsMatchTestData("*", "", true),
            new IsMatchTestData("*", ";", true),

            new IsMatchTestData(";*", "", false),
            new IsMatchTestData(";*", ";", true),
            new IsMatchTestData(";*", "@;", false),
            new IsMatchTestData(";*", ";@", true),

            new IsMatchTestData("*;", "", false),
            new IsMatchTestData("*;", ";", true),
            new IsMatchTestData("*;", "@;", true),
            new IsMatchTestData("*;", ";@", false),

            new IsMatchTestData(";;", "", false),
            new IsMatchTestData(";;", ";;", true),

            new IsMatchTestData(";;*", "", false),
            new IsMatchTestData(";;*", ";;@", true),
            new IsMatchTestData(";;*", ";;", true),

            new IsMatchTestData(";*;", "", false),
            new IsMatchTestData(";*;", ";;", true),
            new IsMatchTestData(";*;", ";@;", true),
            new IsMatchTestData(";*;", ";@;@", false),

            new IsMatchTestData("*;;", "", false),
            new IsMatchTestData("*;;", "@;;", true),
            new IsMatchTestData("*;;", "@;", false),

            new IsMatchTestData("**", "", true),
            new IsMatchTestData("**", ";;;;;;;", true),

            new IsMatchTestData("**;", "", false),
            new IsMatchTestData("**;", ";", true),
            new IsMatchTestData("**;", "@@;", true),

            new IsMatchTestData("*;*", "", false),
            new IsMatchTestData("*;*", "@@@@@;@", true),

            new IsMatchTestData(";**", "", false),
            new IsMatchTestData(";**", ";@@@@@", true),

            new IsMatchTestData(";;**", "", false),
            new IsMatchTestData(";;**", ";;@@@@", true),

            new IsMatchTestData(";*;*", "", false),
            new IsMatchTestData(";*;*", ";@@;@", true),
            new IsMatchTestData(";*;*", ";@@;@", true),

            new IsMatchTestData(";**;", "", false),
            new IsMatchTestData(";**;", ";@@@@@;", true),

            new IsMatchTestData("*;;*", "", false),
            new IsMatchTestData("*;;*", "@@@@;;@@@@", true),
            new IsMatchTestData("*;;*", "@@@@;@;@@@@", false),

            new IsMatchTestData("*;*;", "", false),
            new IsMatchTestData("*;*;", "@@@;@;", true),
            new IsMatchTestData("*;*;", "@@@;@;@", false),

            new IsMatchTestData("**;;", "", false),
            new IsMatchTestData("**;;", "@@@@;;", true),
            new IsMatchTestData("**;;", "@@@@;;@", false),


            new IsMatchTestData("***", "", true),
            new IsMatchTestData("***", "@@@@", true),

            new IsMatchTestData("*;**", "", false),
            new IsMatchTestData("*;**", "@;@@@", true),
            new IsMatchTestData("*;**", "@@@@", false),
            new IsMatchTestData("*;**", ";@@@@@", true),
            new IsMatchTestData("*;**", "@@@@@;", true),

            new IsMatchTestData("*;*;*", "", false),
            new IsMatchTestData("*;*;*", "@@@@;@@;@", true),
            new IsMatchTestData("*;*;*", "@@@@;@@;", true),
            new IsMatchTestData("*;*;*", ";@@;", true),
            new IsMatchTestData("*;*;*", ";;", true),
            new IsMatchTestData("*;*;*", ";@@@@@@", false),

            new IsMatchTestData(";*;*;*;", "", false),
            new IsMatchTestData(";*;*;*;", ";;;;", true),
            new IsMatchTestData(";*;*;*;", ";;;;@", false),
            new IsMatchTestData(";*;*;*;", "@;;;;", false),
            new IsMatchTestData(";*;*;*;", ";@@@;@@;;", true),

            // Strings with no double stars and no slashes match endings.
            new IsMatchTestData("X", "X", true),
            new IsMatchTestData("X", "XA", false),
            new IsMatchTestData("X", "A/X", true),
            new IsMatchTestData("X", "A/XA", false),
            new IsMatchTestData("X", "A/B/X", true),
            new IsMatchTestData("X", "A/B/XA", false),

            // Strings with preceeding slashes match beginnings.
            new IsMatchTestData("/X", "X", true),
            new IsMatchTestData("/X", "XA", false),
            new IsMatchTestData("/X", "A/X", false),
            new IsMatchTestData("/X", "A/XA", false),
            new IsMatchTestData("/X", "A/B/X", false),
            new IsMatchTestData("/X", "A/B/XA", false),

            // "./" is not the same as "/". "." is interpreted as a literal
            // character, and is prone to matching nothing.
            new IsMatchTestData("./X", "X", false),
            new IsMatchTestData("./X", "XA", false),
            new IsMatchTestData("./X", "A/X", false),
            new IsMatchTestData("./X", "A/XA", false),
            new IsMatchTestData("./X", "A/B/X", false),
            new IsMatchTestData("./X", "A/B/XA", false),

            // Strings with stars and no slashes match endings.
            new IsMatchTestData("X*", "X", true),
            new IsMatchTestData("X*", "XA", true),
            new IsMatchTestData("X*", "A/X", true),
            new IsMatchTestData("X*", "A/XA", true),
            new IsMatchTestData("X*", "A/B/X", true),
            new IsMatchTestData("X*", "A/B/XA", true),
            new IsMatchTestData("*X", "X", true),
            new IsMatchTestData("*X", "XA", false),
            new IsMatchTestData("*X", "AX", true),
            new IsMatchTestData("*X", "A/X", true),
            new IsMatchTestData("*X", "A/XA", false),
            new IsMatchTestData("*X", "A/AX", true),
            new IsMatchTestData("*X", "A/B/X", true),
            new IsMatchTestData("*X", "A/B/XA", false),
            new IsMatchTestData("*X", "A/B/AX", true),

            // Strings with preceeding slashes and stars match beginnings.
            new IsMatchTestData("/X*", "X", true),
            new IsMatchTestData("/X*", "XA", true),
            new IsMatchTestData("/X*", "A/X", false),
            new IsMatchTestData("/X*", "A/XA", false),
            new IsMatchTestData("/X*", "A/B/X", false),
            new IsMatchTestData("/X*", "A/B/XA", false),
            new IsMatchTestData("/*X", "X", true),
            new IsMatchTestData("/*X", "XA", false),
            new IsMatchTestData("/*X", "AX", true),
            new IsMatchTestData("/*X", "A/X", false),
            new IsMatchTestData("/*X", "A/XA", false),
            new IsMatchTestData("/*X", "A/AX", false),
            new IsMatchTestData("/*X", "A/B/X", false),
            new IsMatchTestData("/*X", "A/B/XA", false),
            new IsMatchTestData("/*X", "A/B/AX", false),

            // Double stars match file paths
            new IsMatchTestData("X**", "X", true),
            new IsMatchTestData("X**", "XA", true),
            new IsMatchTestData("X**", "A/X", true),
            new IsMatchTestData("X**", "A/XA", true),
            new IsMatchTestData("X**", "A/B/X", true),
            new IsMatchTestData("X**", "A/B/XA", true),
            new IsMatchTestData("X**", "X/A/B/XA", true),
            new IsMatchTestData("**X", "X", true),
            new IsMatchTestData("**X", "XA", false),
            new IsMatchTestData("**X", "AX", true),
            new IsMatchTestData("**X", "A/X", true),
            new IsMatchTestData("**X", "A/XA", false),
            new IsMatchTestData("**X", "A/AX", true),
            new IsMatchTestData("**X", "A/B/X", true),
            new IsMatchTestData("**X", "A/B/XA", false),
            new IsMatchTestData("**X", "A/B/AX", true),
            new IsMatchTestData("**/X", "X", true),
            new IsMatchTestData("**/X", "XA", false),
            new IsMatchTestData("**/X", "AX", false),
            new IsMatchTestData("**/X", "A/X", true),
            new IsMatchTestData("**/X", "A/XA", false),
            new IsMatchTestData("**/X", "A/AX", false),
            new IsMatchTestData("**/X", "A/B/X", true),
            new IsMatchTestData("**/X", "A/B/XA", false),
            new IsMatchTestData("**/X", "A/B/AX", false),

            // Double stars match file paths
            new IsMatchTestData("A/**/X", "X", false),
            new IsMatchTestData("A/**/X", "XA", false),
            new IsMatchTestData("A/**X", "AX", false),
            new IsMatchTestData("A/**X", "A/X", true),
            new IsMatchTestData("A/**X", "A/XA", false),
            new IsMatchTestData("A/**X", "A/AX", true),
            new IsMatchTestData("A/**X", "A/B/X", false),
            new IsMatchTestData("A/**X", "A/B/XA", false),
            new IsMatchTestData("A/**X", "A/B/AX", false),
            new IsMatchTestData("A/**/X", "X", false),
            new IsMatchTestData("A/**/X", "XA", false),
            new IsMatchTestData("A/**/X", "AX", false),
            new IsMatchTestData("A/**/X", "A/X", true),
            new IsMatchTestData("A/**/X", "A/XA", false),
            new IsMatchTestData("A/**/X", "A/AX", false),
            new IsMatchTestData("A/**/X", "A/B/X", true),
            new IsMatchTestData("A/**/X", "A/B/AX", false),
            new IsMatchTestData("A/**/X", "A/B/XA", false),
            new IsMatchTestData("A/**/X", "A/B/AX", false),

            // Weird patterns
            new IsMatchTestData("**/A", "A", true),
            new IsMatchTestData("**/A", "X/A", true),
            new IsMatchTestData("**/A", "XA", false),
            new IsMatchTestData("**/A", "B", false),
            new IsMatchTestData("**/A", "X/B", false),

            new IsMatchTestData("**/**/A", "X", false),
            new IsMatchTestData("**/**/A", "X/A", true),
            new IsMatchTestData("**/**/A", "XA", false),
            new IsMatchTestData("**/**/A", "B", false),
            new IsMatchTestData("**/**/A", "X/B", false),

            new IsMatchTestData("A/**/**", "A", false),
            new IsMatchTestData("A/**/**", "X/A", false),
            new IsMatchTestData("A/**/**", "XA", false),
            new IsMatchTestData("A/**/**", "B", false),
            new IsMatchTestData("A/**/**", "X/B", false),
            new IsMatchTestData("A/**/**", "A/X/A", true),
            new IsMatchTestData("A/**/**", "A/XA", true),
            new IsMatchTestData("A/**/**", "A/B", true),
            new IsMatchTestData("A/**/**", "A/X/B", true),

            new IsMatchTestData("A/**/**/B", "A", false),
            new IsMatchTestData("A/**/**/B", "X/A", false),
            new IsMatchTestData("A/**/**/B", "XA", false),
            new IsMatchTestData("A/**/**/B", "B", false),
            new IsMatchTestData("A/**/**/B", "X/B", false),
            new IsMatchTestData("A/**/**/B", "A/X/A", false),
            new IsMatchTestData("A/**/**/B", "A/XA", false),
            new IsMatchTestData("A/**/**/B", "A/B", true),
            new IsMatchTestData("A/**/**/B", "A/X/B", true),
            new IsMatchTestData("A/**/**/B", "A/X/BC", false),
            new IsMatchTestData("A/**/**/B", "A/X/CB", false),
            new IsMatchTestData("A/**/**/B", "AC/X/B", false),
            new IsMatchTestData("A/**/**/B", "CA/X/B", false),
            new IsMatchTestData("A/**/**/B", "AB", false),

            new IsMatchTestData("**/**.A", "A", false),
            new IsMatchTestData("**/**.A", ".A", true),
            new IsMatchTestData("**/**.A", "X/.A", true),
            new IsMatchTestData("**/**/A", "X.A", false),
            new IsMatchTestData("**/**/A", "B", false),
            new IsMatchTestData("**/**/A", "X/B", false),

            new IsMatchTestData("/", "A", false),
            new IsMatchTestData("/", "A/", false),
            new IsMatchTestData("/", "", true, Kind: GitIgnoreRegexKind.Directory),
        };

        [Test, TestCaseSource(nameof(IsMatchTestSource))]
        public static void IsMatchTest(IsMatchTestData data)
        {
            var regex = GitIgnoreRegex.FromSpan(data.Pattern);
            Assert.That(regex.Unsigned.IsMatch(data.Test), Is.EqualTo(data.Match));

            if (data.Kind != null)
            {
                Assert.That(regex.Unsigned.Kind, Is.EqualTo(data.Kind.Value));
            }

            Assert.That(regex.Sign, Is.EqualTo(data.Sign));
        }
    }
}