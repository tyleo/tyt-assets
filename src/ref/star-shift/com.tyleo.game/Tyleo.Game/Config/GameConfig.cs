#nullable enable

using Tyleo.Collections;

namespace Tyleo.Game.Config
{
    /// <summary>
    /// Tyleo game configuration.
    /// </summary>
    public sealed record GameConfig(
        /// <summary>
        /// The log include patterns.
        /// </summary>
        ReadOnlyArray<string> LogInclude
    )
    {
        /// <summary>
        /// The default Tyleo game configuration.
        /// </summary>
        public static readonly GameConfig Default = new(
            LogInclude: ReadOnlyArray.FromParams(
                "/Tyleo/Debug",
                "/Tyleo/DisposeReminder",
                "/Tyleo/Initialization",
                "/Tyleo.Game/Loggers"
            )
        );
    }
}