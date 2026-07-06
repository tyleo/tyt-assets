#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Arguments for fixed updates.
    /// </summary>
    public readonly struct FixedUpdateArgs
    {
        /// <summary>
        /// Information about the timing of the current frame.
        /// </summary>
        public readonly FixedFrameTimes FrameTimes;

        public FixedUpdateArgs(
            FixedFrameTimes frameTimes
        )
        {
            FrameTimes = frameTimes;
        }
    }
}
