#nullable enable

using System;
using System.IO;
using Tyleo.Game.Config;

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Logic for editor and runtime updates.
    /// </summary>
    internal static class EditorAndRuntimeUpdateLogic
    {
        /// <summary>
        /// Updates StarShift from the editor and runtime context.
        /// </summary>
        /// <param name="args">The arguments for the update.</param>
        public static void Update(in EditorAndRuntimeUpdateArgs args)
        {
            GameModule.Log.Update.EditorAndRuntime.Info(
                "EditorAndRuntimeUpdate"
            );

            UpdateConfigFile(args);
        }

        private static void UpdateConfigFile(
            in EditorAndRuntimeUpdateArgs args
        )
        {
            ref readonly var frameTimes = ref args.FrameTimes;

            // Return early if we haven't reached the accumulated time.
            if (frameTimes.AccumulatedSeconds <= args.State.NextConfigFileCheckAccumulatedSeconds)
            {
                return;
            }

            // Schedule the next config file check.
            args.State.NextConfigFileCheckAccumulatedSeconds =
                frameTimes.AccumulatedSeconds + GameConst.ConfigFileCheckIntervalSeconds;

            var lastKnownConfigChangeTimeUtc = args.State.LastKnownConfigChangeTimeUtc;
            var lastConfigChangeTimeUtc = GameModule.Instance.Dependencies.GetLastConfigChangeTimeUtc();

            if (lastKnownConfigChangeTimeUtc == lastConfigChangeTimeUtc)
            {
                // The config file has not changed.
                return;
            }

            GameModule.Instance.Dependencies.ReloadConfig();
            args.State.LastKnownConfigChangeTimeUtc = lastConfigChangeTimeUtc;
        }
    }
}