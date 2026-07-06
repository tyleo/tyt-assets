#nullable enable

using System;
using System.IO;

namespace Tyleo.GitIgnore
{
    /// <summary>
    /// Provides utility methods for working with
    /// <see cref="Tyleo.GitIgnore"/> types.
    /// </summary>
    public static class GitIgnoreExt
    {
        /// <summary>
        /// Aggregates signed directory matches across a set of
        /// patterns. Later matches override earlier ones.
        /// </summary>
        /// <param name="self">The signed patterns.</param>
        /// <param name="path">The directory path to match.</param>
        /// <returns>
        /// <c>true</c> if included, <c>false</c> if excluded,
        /// or <c>null</c> if no pattern matched.
        /// </returns>
        public static bool? IsDirectoryMatch(
            this GitIgnoreRegex[] self,
            string path
        ) => IsDirectoryMatch(self.AsSpan(), path);

        /// <inheritdoc cref="IsDirectoryMatch(GitIgnoreRegex[], string)"/>
        public static bool? IsDirectoryMatch(
            this ReadOnlySpan<GitIgnoreRegex> self,
            string path
        )
        {
            bool? isMatch = default;
            for (var i = 0; i < self.Length; i++)
            {
                isMatch = self[i].IsMatch(path) ?? isMatch;
            }
            return isMatch;
        }

        /// <summary>
        /// Determines whether any of the regexes match the directory
        /// path.
        /// </summary>
        /// <param name="self">
        /// The array of <see cref="UnsignedGitIgnoreRegex"/> instances.
        /// </param>
        /// <param name="path">The directory path to match.</param>
        /// <returns>
        /// True if any regex matches the directory path; otherwise,
        /// false.
        /// </returns>
        public static bool IsDirectoryMatch(
            this UnsignedGitIgnoreRegex[] self,
            string path
        ) => IsDirectoryMatch(self.AsSpan(), path);

        /// <summary>
        /// Determines whether any of the regexes match the directory
        /// path.
        /// </summary>
        /// <param name="self">
        /// The array of <see cref="UnsignedGitIgnoreRegex"/> instances.
        /// </param>
        /// <param name="path">The directory path to match.</param>
        /// <returns>
        /// True if any regex matches the directory path; otherwise,
        /// false.
        /// </returns>
        public static bool IsDirectoryMatch(
            this ReadOnlySpan<UnsignedGitIgnoreRegex> self,
            string path
        )
        {
            for (var i = 0; i < self.Length; i++)
            {
                if (self[i].IsMatch(path)) return true;
            }
            return false;
        }

        /// <summary>
        /// Aggregates signed file matches across a set of
        /// patterns, skipping directory-only patterns. Later
        /// matches override earlier ones.
        /// </summary>
        /// <param name="self">The signed patterns.</param>
        /// <param name="path">The file path to match.</param>
        /// <returns>
        /// <c>true</c> if included, <c>false</c> if excluded,
        /// or <c>null</c> if no pattern matched.
        /// </returns>
        public static bool? IsFileMatch(
            this GitIgnoreRegex[] self,
            string path
        ) => IsFileMatch(self.AsSpan(), path);

        /// <inheritdoc cref="IsFileMatch(GitIgnoreRegex[], string)"/>
        public static bool? IsFileMatch(
            this ReadOnlySpan<GitIgnoreRegex> self,
            string path
        )
        {
            bool? isMatch = default;
            for (var i = 0; i < self.Length; i++)
            {
                if (self[i].Unsigned.Kind == GitIgnoreRegexKind.Directory)
                {
                    continue;
                }
                isMatch = self[i].IsMatch(path) ?? isMatch;
            }
            return isMatch;
        }

        /// <summary>
        /// Determines whether any of the regexes match the file path.
        /// </summary>
        /// <param name="self">
        /// The array of <see cref="UnsignedGitIgnoreRegex"/> instances.
        /// </param>
        /// <param name="path">The file path to match.</param>
        /// <returns>
        /// True if any regex matches the file path; otherwise, false.
        /// </returns>
        public static bool IsFileMatch(
            this UnsignedGitIgnoreRegex[] self,
            string path
        ) => IsFileMatch(self.AsSpan(), path);

        /// <summary>
        /// Determines whether any of the regexes match the file path.
        /// </summary>
        /// <param name="self">
        /// The array of <see cref="UnsignedGitIgnoreRegex"/> instances.
        /// </param>
        /// <param name="path">The file path to match.</param>
        /// <returns>
        /// True if any regex matches the file path; otherwise, false.
        /// </returns>
        public static bool IsFileMatch(
            this ReadOnlySpan<UnsignedGitIgnoreRegex> self,
            string path
        )
        {
            for (var i = 0; i < self.Length; i++)
            {
                if (self[i].Kind == GitIgnoreRegexKind.Directory) continue;
                if (self[i].IsMatch(path)) return true;
            }
            return false;
        }

        /// <summary>
        /// Walks each directory component of the path, checking
        /// directory matches at each level, then checks the file.
        /// Later matches override earlier ones.
        /// </summary>
        /// <param name="self">The signed patterns.</param>
        /// <param name="path">The full path to match.</param>
        /// <returns>
        /// <c>true</c> if included, <c>false</c> if excluded,
        /// or <c>null</c> if no pattern matched.
        /// </returns>
        public static bool? IsPathMatch(
            this GitIgnoreRegex[] self,
            string path
        ) => IsPathMatch(self.AsSpan(), path);

        /// <inheritdoc cref="IsPathMatch(GitIgnoreRegex[], string)"/>
        public static bool? IsPathMatch(
            this ReadOnlySpan<GitIgnoreRegex> self,
            string path
        )
        {
            var pathParts = path.Split(
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            );

            var currentPath = pathParts[0];
            var isDirectoryMatch = self.IsDirectoryMatch(currentPath);

            for (
                var i = 1;
                i < pathParts.Length && isDirectoryMatch != false;
                i++
            )
            {
                currentPath = Path.Combine(currentPath, pathParts[i]);
                var current = self.IsDirectoryMatch(currentPath);
                isDirectoryMatch = current ?? isDirectoryMatch;
            }

            if (isDirectoryMatch == false) return false;

            var isFileMatch = self.IsFileMatch(path);
            if (isFileMatch == false) return false;

            if (isDirectoryMatch == null && isFileMatch == null) return null;

            return true;
        }

        /// <summary>
        /// Walks each directory component of the path, checking
        /// directory matches at each level, then checks the file.
        /// </summary>
        /// <param name="self">
        /// The array of
        /// <see cref="UnsignedGitIgnoreRegex"/> instances.
        /// </param>
        /// <param name="path">The full path to match.</param>
        /// <returns>
        /// True if any regex matches a directory component or the
        /// file; otherwise, false.
        /// </returns>
        public static bool IsPathMatch(
            this UnsignedGitIgnoreRegex[] self,
            string path
        ) => IsPathMatch(self.AsSpan(), path);

        /// <summary>
        /// Walks each directory component of the path, checking
        /// directory matches at each level, then checks the file.
        /// </summary>
        /// <param name="self">
        /// The array of
        /// <see cref="UnsignedGitIgnoreRegex"/> instances.
        /// </param>
        /// <param name="path">The full path to match.</param>
        /// <returns>
        /// True if any regex matches a directory component or the
        /// file; otherwise, false.
        /// </returns>
        public static bool IsPathMatch(
            this ReadOnlySpan<UnsignedGitIgnoreRegex> self,
            string path
        )
        {
            var pathParts = path.Split(
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar
            );

            var currentPath = pathParts[0];
            if (self.IsDirectoryMatch(currentPath)) return true;

            for (var i = 1; i < pathParts.Length; i++)
            {
                currentPath = Path.Combine(currentPath, pathParts[i]);
                if (self.IsDirectoryMatch(currentPath)) return true;
            }

            return self.IsFileMatch(path);
        }
    }
}
