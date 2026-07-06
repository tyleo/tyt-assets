#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Arguments for editor and runtime updates.
    /// </summary>
    public readonly struct EditorAndRuntimeUpdateArgs
    {
        /// <summary>
        /// The state for editor and runtime updates.
        /// </summary>
        public readonly EditorAndRuntimeUpdateState State;

        /// <summary>
        /// Information about the timing of the current frame.
        /// </summary>
        public readonly FrameTimes FrameTimes;

        public EditorAndRuntimeUpdateArgs(
            EditorAndRuntimeUpdateState state,
            FrameTimes frameTimes
        )
        {
            State = state;
            FrameTimes = frameTimes;
        }
    }
}
