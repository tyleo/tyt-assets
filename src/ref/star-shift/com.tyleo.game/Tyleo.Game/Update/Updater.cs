#nullable enable

using System.Diagnostics;
using Tyleo.Logging;

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Updater for the module instance.
    /// </summary>
    internal struct Updater
    {
        private const float EditorUpdateIntervalSeconds = 1f / 60f;

        private ExecutionContext? _updateContext;

        private TimeTracker _editorTimeTracker;
        private FixedTimeTracker _fixedTimeTracker;

        public static Updater New(
            ExecutionContext updateContext
        ) => new()
        {
            _updateContext = updateContext,
            _editorTimeTracker = TimeTracker.New()
        };

        /// <summary>
        /// Updates StarShift from the editor context.
        /// </summary>
        /// <param name="editorAndRuntimeUpdateState">
        /// The state for editor and runtime updates.
        /// </param>
        /// <param name="editorUpdatable">
        /// The editor-only updatable to update.
        /// </param>
        public void EditorUpdate(
            EditorAndRuntimeUpdateState editorAndRuntimeUpdateState,
            IEditorUpdatable editorUpdatable
        )
        {
            if (_updateContext != ExecutionContext.Editor) return;

            if (!_editorTimeTracker.RanFirstFrame)
            {
                // First update call, start the stopwatch.
                _editorTimeTracker.Stopwatch.Start();

                var firstFrameTimes = new FrameTimes(
                    accumulatedSeconds: 0f,
                    deltaSeconds: 0f,
                    frameIndex: 0
                );

                GameModule.Log.Update.Editor.Info(
                    "EditorUpdate: 0"
                );

                EditorAndRuntimeUpdateLogic.Update(new(
                    state: editorAndRuntimeUpdateState,
                    frameTimes: firstFrameTimes
                ));

                GameModule.Log.Update.EditorOnly.Info(
                    "EditorOnlyUpdate"
                );

                editorUpdatable.EditorUpdate(new(
                    frameTimes: firstFrameTimes
                ));

                _editorTimeTracker.RanFirstFrame = true;

                return;
            }

            // Return early if we haven't reached the update interval.
            var deltaSeconds = (float)_editorTimeTracker.Stopwatch.Elapsed.TotalSeconds;
            if (deltaSeconds < EditorUpdateIntervalSeconds) return;

            _editorTimeTracker.Stopwatch.Restart();

            _editorTimeTracker.AccumulatedSeconds += deltaSeconds;
            _editorTimeTracker.FrameIndex++;

            GameModule.Log.Update.Editor.IfEnabled()?.Info(
                $"EditorUpdate: {_editorTimeTracker.FrameIndex}"
            );

            var frameTimes = new FrameTimes(
                accumulatedSeconds: _editorTimeTracker.AccumulatedSeconds,
                deltaSeconds: deltaSeconds,
                frameIndex: _editorTimeTracker.FrameIndex
            );

            EditorAndRuntimeUpdateLogic.Update(new(
                state: editorAndRuntimeUpdateState,
                frameTimes: frameTimes
            ));

            GameModule.Log.Update.EditorOnly.Info(
                "EditorOnlyUpdate"
            );

            editorUpdatable.EditorUpdate(new(
                frameTimes: frameTimes
            ));
        }

        /// <summary>
        /// Updates StarShift from the runtime context.
        /// </summary>
        /// <param name="frameTimes">
        /// The frame times for the update.
        /// </param>
        /// <param name="editorAndRuntimeUpdateState">
        /// The state for editor and runtime updates.
        /// </param>
        /// <param name="runtime">
        /// The runtime to update.
        /// </param>
        public void RuntimeUpdate(
            in FrameTimes frameTimes,
            EditorAndRuntimeUpdateState editorAndRuntimeUpdateState,
            GameRuntimeInstance runtime
        )
        {
            if (_updateContext != ExecutionContext.Runtime) return;

            GameModule.Log.Update.Runtime.IfEnabled()?.Info(
                $"RuntimeUpdate: {frameTimes.FrameIndex}"
            );

            EditorAndRuntimeUpdateLogic.Update(new(
                state: editorAndRuntimeUpdateState,
                frameTimes: frameTimes
            ));

            GameModule.Log.Update.RuntimeOnly.Info(
                "RuntimeOnlyUpdate"
            );

            runtime.Update(
                args: new(
                    frameTimes: frameTimes
                )
            );
        }

        /// <summary>
        /// Updates StarShift from the fixed runtime context.
        /// </summary>
        /// <param name="fixedDeltaTime">
        /// The fixed delta time for this update.
        /// </param>
        /// <param name="runtime">
        /// The runtime to update.
        /// </param>
        public void FixedUpdate(
            float fixedDeltaTime,
            GameRuntimeInstance runtime
        )
        {
            if (_updateContext != ExecutionContext.Runtime) return;
            GameModule.Log.Update.Fixed.Info(
                "FixedUpdate"
            );

            if (!_fixedTimeTracker.RanFirstFrame)
            {
                var firstFrameTimes = new FixedFrameTimes(
                    fixedAccumulatedSeconds: fixedDeltaTime,
                    fixedDeltaSeconds: fixedDeltaTime,
                    fixedFrameIndex: _fixedTimeTracker.FrameIndex,
                    variableAccumulatedSeconds: _editorTimeTracker.AccumulatedSeconds,
                    variableFrameIndex: _editorTimeTracker.FrameIndex
                );

                runtime.FixedUpdate(
                    args: new(
                        frameTimes: firstFrameTimes
                    )
                );

                _fixedTimeTracker.RanFirstFrame = true;

                return;
            }

            _fixedTimeTracker.FrameIndex++;
            var accumulatedSeconds =
                _fixedTimeTracker.FrameIndex *
                fixedDeltaTime;

            var frameTimes = new FixedFrameTimes(
                fixedAccumulatedSeconds: accumulatedSeconds,
                fixedDeltaSeconds: fixedDeltaTime,
                fixedFrameIndex: _fixedTimeTracker.FrameIndex,
                variableAccumulatedSeconds: _editorTimeTracker.AccumulatedSeconds,
                variableFrameIndex: _editorTimeTracker.FrameIndex
            );

            runtime.FixedUpdate(
                args: new(
                    frameTimes: frameTimes
                )
            );
        }

        /// <summary>
        /// Notifies the updater that the edit mode has been entered.
        /// </summary>
        public void NotifyEnteredEditMode()
        {
            _updateContext = ExecutionContext.Editor;
        }

        /// <summary>
        /// Notifies the updater that the play mode has been entered.
        /// </summary>
        public void NotifyEnteredPlayMode()
        {
            _updateContext = ExecutionContext.Runtime;
        }

        /// <summary>
        /// Notifies the updater that the edit mode is exiting.
        /// </summary>
        public void NotifyExitingEditMode()
        {
            _editorTimeTracker.Clear();
            _updateContext = null;
        }

        /// <summary>
        /// Notifies the updater that the play mode is exiting.
        /// </summary>
        public void NotifyExitingPlayMode()
        {
            _fixedTimeTracker.Clear();
            _updateContext = null;
        }

        private struct TimeTracker
        {
            public Stopwatch Stopwatch;
            public float AccumulatedSeconds;
            public int FrameIndex;
            public bool RanFirstFrame;

            public static TimeTracker New() => new()
            {
                Stopwatch = new Stopwatch(),
                AccumulatedSeconds = 0f,
                FrameIndex = 0,
                RanFirstFrame = false
            };

            public void Clear()
            {
                Stopwatch.Reset();
                AccumulatedSeconds = 0f;
                FrameIndex = 0;
                RanFirstFrame = false;
            }
        }

        private struct FixedTimeTracker
        {
            public int FrameIndex;
            public bool RanFirstFrame;

            public static FixedTimeTracker New() => new()
            {
                FrameIndex = 0,
                RanFirstFrame = false
            };

            public void Clear()
            {
                FrameIndex = 0;
                RanFirstFrame = false;
            }
        }
    }
}