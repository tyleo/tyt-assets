#nullable enable

namespace Tyleo.GitIgnore
{
    /// <summary>
    /// Represents the kind of a Git ignore regex.
    /// </summary>
    public enum GitIgnoreRegexKind
    {
        /// <summary>
        /// Matches only directories.
        /// </summary>
        Directory,

        /// <summary>
        /// Matches files or directories.
        /// </summary>
        FileOrDirectory
    }
}
