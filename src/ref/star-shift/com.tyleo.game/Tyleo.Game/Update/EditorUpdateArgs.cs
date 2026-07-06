#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Arguments for editor only updates.
    /// </summary>
    public readonly struct EditorUpdateArgs
    {
        /// <summary>
        /// Information about the timing of the current frame.
        /// </summary>
        public readonly FrameTimes FrameTimes;

        public EditorUpdateArgs(
            FrameTimes frameTimes
        )
        {
            FrameTimes = frameTimes;
        }
    }
}
