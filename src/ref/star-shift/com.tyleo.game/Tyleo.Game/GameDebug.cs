#nullable enable

namespace Tyleo.Game
{
    /// <summary>
    /// Provides debug drawing utilities.
    /// </summary>
    public sealed class GameDebug
    {
        private readonly IGameDependencies _dependencies;

        internal GameDebug(IGameDependencies dependencies)
        {
            _dependencies = dependencies;
        }
        /// <summary>
        /// Draws a vector from <paramref name="tail"/> to
        /// <paramref name="head"/> with the specified color and duration.
        /// </summary>
        /// <param name="tail">The start of the vector.</param>
        /// <param name="head">The end of the vector.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="duration">
        /// How long the line should be visible, in seconds. A duration of 0
        /// means the line is visible for one frame.
        /// </param>
        public void DrawVector(
            TyVector3 tail,
            TyVector3 head,
            TyRgbaColor? color = null,
            float duration = 0f
        ) => _dependencies.DrawLine(tail, head, color ?? new(0f, 1f, 0f, 1f), duration);
    }
}
