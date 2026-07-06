#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Arguments for runtime only updates.
    /// </summary>
    public readonly struct RuntimeUpdateArgs
    {
        /// <summary>
        /// Information about the timing of the current frame.
        /// </summary>
        public readonly FrameTimes FrameTimes;

        public RuntimeUpdateArgs(
            FrameTimes frameTimes
        )
        {
            FrameTimes = frameTimes;
        }
    }
}
