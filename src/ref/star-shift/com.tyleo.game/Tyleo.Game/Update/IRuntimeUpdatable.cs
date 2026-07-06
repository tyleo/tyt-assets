#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Represents an object that can only be updated at runtime.
    /// </summary>
    public interface IRuntimeUpdatable
    {
        /// <summary>
        /// Performs an update.
        /// </summary>
        /// <param name="args">
        /// Arguments providing context for the runtime update.
        /// </param>
        void RuntimeUpdate(in RuntimeUpdateArgs args);
    }
}