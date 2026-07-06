#nullable enable

using System;
using Tyleo.Collections;
using Tyleo.Extensions;
using Tyleo.Game.Config;
using Tyleo.Game.Update;
using Tyleo.GitIgnore;
using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// Represents this assembly.
    /// </summary>
    public sealed class GameModuleInstance
    {
        private readonly IGameDependencies _dependencies;
        private readonly ILogSys _logSys;
        private EditorAndRuntimeUpdateState _editorUpdateState;
        private Updater _updater;
        private GameRuntimeInstance? _runtimeInstance = null;

        /// <summary>
        /// Debug drawing utilities.
        /// </summary>
        public GameDebug Debug { get; }

        /// <summary>
        /// The dependencies for the module.
        /// </summary>
        internal IGameDependencies Dependencies => _dependencies;

        /// <summary>
        /// Indicates whether the runtime instance is initialized.
        /// </summary>
        public bool IsRuntimeInitialized => _runtimeInstance != null;

        public GameRuntimeInstance? Runtime => _runtimeInstance;

        private GameModuleInstance(
            GameDebug debug,
            IGameDependencies dependencies,
            ILogSys logSys,
            EditorAndRuntimeUpdateState editorAndRuntimeUpdateState,
            Updater updater
        )
        {
            Debug = debug;
            _dependencies = dependencies;
            _logSys = logSys;
            _editorUpdateState = editorAndRuntimeUpdateState;
            _updater = updater;
        }

        internal static GameModuleInstance New(
            IGameDependencies dependencies,
            ExecutionContext initializationContext,
            ILogSys logSys,
            DateTime? lastKnownConfigChangeTimeUtc
        ) => new(
            debug: new GameDebug(dependencies),
            dependencies: dependencies,
            logSys: logSys,
            editorAndRuntimeUpdateState: new EditorAndRuntimeUpdateState
            {
                LastKnownConfigChangeTimeUtc = lastKnownConfigChangeTimeUtc
            },
            updater: Updater.New(initializationContext)
        );

        /// <summary>
        /// Initializes the runtime instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The runtime instance is already initialized.
        /// </exception>
        public void InitializeRuntime()
        {
            if (_runtimeInstance != null) throw new InvalidOperationException(
                "Runtime instance is already initialized."
            );

            _runtimeInstance = GameRuntimeInstance.New(
                rootBehaviour: _dependencies.MaterializeRootBehaviour()
            );

            GameModule.Log.RuntimeInitialization.Info("Runtime did initialize.");
        }

        /// <summary>
        /// Notifies the module that the editor has entered edit mode.
        /// </summary>
        public void NotifyEnteredEditMode()
        {
            _updater.NotifyEnteredEditMode();
        }

        /// <summary>
        /// Notifies the module that the editor has entered play mode.
        /// </summary>
        public void NotifyEnteredPlayMode()
        {
            _updater.NotifyEnteredPlayMode();
        }

        /// <summary>
        /// Notifies the module that the editor is exiting edit mode.
        /// </summary>
        public void NotifyExitingEditMode()
        {
            _updater.NotifyExitingEditMode();

            _editorUpdateState.NextConfigFileCheckAccumulatedSeconds =
                GameConst.ConfigFileCheckIntervalSeconds;
        }

        /// <summary>
        /// Notifies the module that the editor is exiting play mode.
        /// </summary>
        public void NotifyExitingPlayMode()
        {
            _updater.NotifyExitingPlayMode();

            if (_runtimeInstance == null) throw new InvalidOperationException(
                "Runtime instance is not initialized."
            );

            GameModule.Log.RuntimeInitialization.Info("Runtime will dispose.");

            _runtimeInstance.Dispose();
            _runtimeInstance = default;

            _editorUpdateState.NextConfigFileCheckAccumulatedSeconds =
                GameConst.ConfigFileCheckIntervalSeconds;
        }

        /// <summary>
        /// Registers the root scene object for the current scene.
        /// </summary>
        /// <param name="rootSceneObject">The root scene object.</param>
        public void RegisterRootSceneObject(
            IRootSceneObject rootSceneObject
        ) => _runtimeInstance!.RetainRootSceneObject(
            rootSceneObject
        );

        /// <summary>
        /// Updates TyleoGame from the editor context.
        /// </summary>
        /// <param name="editorUpdatable">
        /// The editor-only updatable to update.
        /// </param>
        public void EditorUpdate(IEditorUpdatable editorUpdatable) =>
            _updater.EditorUpdate(_editorUpdateState, editorUpdatable);

        /// <summary>
        /// Updates TyleoGame from the runtime context.
        /// </summary>
        /// <param name="frameTimes">
        /// The frame times for the update.
        /// </param>
        public void RuntimeUpdate(
            in FrameTimes frameTimes
        ) => _updater.RuntimeUpdate(
            frameTimes,
            _editorUpdateState,
            _runtimeInstance!
        );

        /// <summary>
        /// Updates TyleoGame from the runtime context.
        /// </summary>
        /// <param name="fixedDeltaTime">
        /// The fixed delta time for this update.
        /// </param>
        public void FixedUpdate(float fixedDeltaTime) => _updater.FixedUpdate(
            fixedDeltaTime,
            _runtimeInstance!
        );

        /// <summary>
        /// Loads a new configuration into the module.
        /// </summary>
        /// <param name="config">The new configuration.</param>
        public void SetConfiguration(GameConfig config)
        {
            ReloadLoggers(_logSys, config.LogInclude);
        }

        internal static void ReloadLoggers(
            ILogSys logSys,
            ReadOnlyArray<string> logInclude
        )
        {
            logSys.DisableAllLoggers();

            // Always enable this logger to see status.
            var logLogger = GameModule.Log.Loggers;
            logLogger.IsEnabled = true;

            logLogger.Info($"Reloading Loggers");

            var includesSpan = logInclude.AsReadOnlySpan();
            var regexes = GitIgnoreRegex.FromSpansIgnoreInert(includesSpan);

            foreach (var logger in logSys.AllLoggers)
            {
                var isMatch = regexes.IsPathMatch(logger.AbsolutePath) ?? false;
                if (!isMatch) continue;
                logger.IsEnabled = true;
            }

            var enabledLoggersString = logSys.GetEnabledLoggersString();
            var statusString = enabledLoggersString.IsEmpty() ?
                "No loggers are enabled." :
                $"Enabled loggers:\n{enabledLoggersString}";

            logLogger.Info(statusString);
        }
    }
}