#nullable enable

namespace Tyleo.Game
{
    /// <summary>
    /// Constants used throughout the game.
    /// </summary>
    public static class GameConst
    {
        /// <summary>
        /// The interval, in seconds, at which to check for changes to the
        /// configuration file.
        /// </summary>
        public const float ConfigFileCheckIntervalSeconds = 1f;

        /// <summary>
        /// The default number of depenetration steps to take.
        /// </summary>
        public const int DefaultDepenetrationStepCount = 3;

        /// <summary>
        /// The default number of depenetration steps to take.
        /// </summary>
        public const float DefaultDepenetrationToleranceSquared = 1e-8f;

        /// <summary>
        /// The default skin width for collisions.
        /// </summary>
        public const float DefaultSkinWidth = 0.01f;

        /// <summary>
        /// The default tolerance for vector operations.
        /// </summary>
        public const float DefaultTolerance = 1e-5f;

        /// <summary>
        /// Square of the <see cref="DefaultTolerance"/>.
        /// </summary>
        public const float DefaultToleranceSquared =
            DefaultTolerance * DefaultTolerance;

        /// <summary>
        /// The threshold for using spherical linear interpolation (slerp) over
        /// linear interpolation (lerp).
        /// </summary>
        public const float SlerpDotThreshold = .9995f;
    }
}