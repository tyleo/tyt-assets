#nullable enable

using System.IO;

namespace Tyleo.GitIgnore
{
    /// <summary>
    /// Provides extension methods for the <see cref="char"/> type.
    /// </summary>
    internal static class CharExt
    {
        /// <summary>
        /// Determines whether the specified character is a directory separator
        /// character.
        /// </summary>
        /// <param name="self">The character to check.</param>
        /// <returns>
        /// <c>true</c> if the character is a directory separator; otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool IsDirectorySeparatorChar(this char self) =>
            self == Path.DirectorySeparatorChar ||
            self == Path.AltDirectorySeparatorChar;

        /// <summary>
        /// Determines whether the specified character is a special regex
        /// character.
        /// </summary>
        /// <param name="self">The character to check.</param>
        /// <returns>
        /// <c>true</c> if the character is a special regex character;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSpecialRegexChar(this char self) =>
        self switch
        {
            '.' => true,
            '+' => true,
            '*' => true,
            '?' => true,
            '^' => true,
            '$' => true,
            '(' => true,
            ')' => true,
            '[' => true,
            ']' => true,
            '{' => true,
            '}' => true,
            '|' => true,
            '\\' => true,
            _ => false
        };
    }
}
