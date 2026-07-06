#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Timing information for a single fixed-update step.
    /// </summary>
    public readonly struct FixedFrameTimes
    {
        /// <summary>
        /// Total accumulated fixed time since start, in seconds.
        /// </summary>
        public readonly float FixedAccumulatedSeconds;

        /// <summary>
        /// Duration of this fixed update step, in seconds.
        /// </summary>
        public readonly float FixedDeltaSeconds;

        /// <summary>
        /// Zero-based index of the fixed update step since start.
        /// </summary>
        public readonly int FixedFrameIndex;

        /// <summary>
        /// The accumulated variable (non-fixed) time since start, in seconds.
        /// </summary>
        public readonly float VariableAccumulatedSeconds;

        /// <summary>
        /// Zero-based index of the variable-rate frame corresponding to this
        /// update.
        /// </summary>
        public readonly int VariableFrameIndex;

        public FixedFrameTimes(
            float fixedAccumulatedSeconds,
            float fixedDeltaSeconds,
            int fixedFrameIndex,
            float variableAccumulatedSeconds,
            int variableFrameIndex
        )
        {
            FixedAccumulatedSeconds = fixedAccumulatedSeconds;
            FixedDeltaSeconds = fixedDeltaSeconds;
            FixedFrameIndex = fixedFrameIndex;
            VariableAccumulatedSeconds = variableAccumulatedSeconds;
            VariableFrameIndex = variableFrameIndex;
        }
    }
}