#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Frame timing information.
    /// </summary>
    public readonly struct FrameTimes
    {
        /// <summary>
        /// The accumulated time for all updates, in seconds.
        /// </summary>
        public readonly float AccumulatedSeconds;

        /// <summary>
        /// The time since the last update, in seconds.
        /// </summary>
        public readonly float DeltaSeconds;

        /// <summary>
        /// The number of frames since the start.
        /// </summary>
        public readonly int FrameIndex;

        public FrameTimes(
            float accumulatedSeconds,
            float deltaSeconds,
            int frameIndex
        )
        {
            AccumulatedSeconds = accumulatedSeconds;
            DeltaSeconds = deltaSeconds;
            FrameIndex = frameIndex;
        }
    }
}