#nullable enable

using System;

namespace Tyleo.Game.Update
{
    /// <summary>
    /// State for editor and runtime updates.
    /// </summary>
    public sealed class EditorAndRuntimeUpdateState
    {
        /// <summary>
        /// The last time the configuration file changed.
        /// </summary>
        public DateTime? LastKnownConfigChangeTimeUtc;

        /// <summary>
        /// The accumulated time for the last config file check, in seconds.
        /// </summary>
        public float NextConfigFileCheckAccumulatedSeconds = 0;
    }
}