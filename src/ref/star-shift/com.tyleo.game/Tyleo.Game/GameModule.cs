#nullable enable

using System;
using Tyleo.Collections;
using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// Holds the module instance that represents this assembly.
    /// </summary>
    public static partial class GameModule
    {
        private static GameModuleInstance? _instance;
        private static GameLoggers? _loggers;
        private static bool _loggerEnablementsInitialized = false;

        /// <summary>
        /// Gets the module instance.
        /// </summary>
        public static GameModuleInstance Instance => _instance!;

        /// <summary>
        /// Gets a value indicating whether the module is initialized.
        /// </summary>
        public static bool IsInitialized => _instance != null;

        /// <summary>
        /// Gets the loggers from this module.
        /// </summary>
        public static GameLoggers Log => _loggers!;

        /// <summary>
        /// Initializes logging for the module.
        /// </summary>
        /// <param name="node">The logger factory node.</param>
        public static void InitializeLoggers(ILoggerFactoryNode node)
        {
            if (_loggers != null) throw new InvalidOperationException(
                $"{nameof(GameModule)} loggers are already initialized."
            );

            _loggers = GameLoggers.FromLoggerNode(node);
        }

        /// <summary>
        /// Initializes logger enablements for all modules.
        /// </summary>
        /// <param name="logSys">The logging system.</param>
        /// <param name="logIncludes">The log includes.</param>
        public static void InitializeLoggerEnablements(
            ILogSys logSys,
            ReadOnlyArray<string> logIncludes
        )
        {
            if (_loggers == null) throw new InvalidOperationException(
                $"{nameof(GameModule)} loggers are not initialized. Call {nameof(InitializeLoggers)}() first."
            );
            if (_loggerEnablementsInitialized) throw new InvalidOperationException(
                $"{nameof(GameModule)} logger enablements are already initialized."
            );

            GameModuleInstance.ReloadLoggers(logSys, logIncludes);
            _loggerEnablementsInitialized = true;
        }

        /// <summary>
        /// Initializes the module.
        /// </summary>
        /// <param name="args">
        /// The initialization arguments.
        /// </param>
        public static void Initialize(in InitializeArgs args)
        {
            if (_loggers == null) throw new InvalidOperationException(
                $"{nameof(GameModule)} loggers are not initialized. Call {nameof(InitializeLoggers)}() first."
            );
            if (!_loggerEnablementsInitialized) throw new InvalidOperationException(
                $"{nameof(GameModule)} logger enablements are not initialized. Call {nameof(InitializeLoggerEnablements)}() first."
            );
            if (_instance != null) throw new InvalidOperationException(
                $"{nameof(GameModule)} is already initialized."
            );

            var initializationContext = args.InitializationContext;

            _instance = GameModuleInstance.New(
                initializationContext: initializationContext,
                dependencies: args.Dependencies,
                logSys: args.LogSys,
                lastKnownConfigChangeTimeUtc: args.LastKnownConfigChangeTimeUtc
            );

            TyleoModule.Log.Initialization.IfEnabled()?.Info(
                $"{nameof(GameModule)}.{nameof(Initialize)}() was called from {initializationContext}."
            );
        }
    }
}