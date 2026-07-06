#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Represents an object that can be updated at fixed intervals during
    /// runtime.
    /// </summary>
    public interface IRuntimeFixedUpdatable
    {
        /// <summary>
        /// Performs a fixed update.
        /// </summary>
        /// <param name="args">
        /// Arguments providing context for the fixed update.
        /// </param>
        void RuntimeFixedUpdate(in FixedUpdateArgs args);
    }
}