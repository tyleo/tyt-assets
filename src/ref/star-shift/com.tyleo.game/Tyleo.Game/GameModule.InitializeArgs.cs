#nullable enable

using System;
using Tyleo.Game.Config;
using Tyleo.Logging;

namespace Tyleo.Game
{
    public static partial class GameModule
    {
        /// <summary>
        /// Arguments for initializing the module.
        /// </summary>
        public struct InitializeArgs
        {
            /// <summary>
            /// The dependencies to use.
            /// </summary>
            public IGameDependencies Dependencies;

            /// <summary>
            /// The execution context used to initialize the module.
            /// </summary>
            public ExecutionContext InitializationContext;

            /// <summary>
            /// The logging system to use.
            /// </summary>
            public ILogSys LogSys;

            /// <summary>
            /// The last write time of the configuration file, if any.
            /// </summary>
            public DateTime? LastKnownConfigChangeTimeUtc;

            /// <summary>
            /// The initial configuration for the game.
            /// </summary>
            public GameConfig InitialConfig;
        }
    }
}