#nullable enable

namespace Tyleo.Game
{
    /// <summary>
    /// Describes the environment in which code is executing.
    /// </summary>
    public enum ExecutionContext
    {
        /// <summary>
        /// Executing in an editor context.
        /// </summary>
        Editor,

        /// <summary>
        /// Executing in a runtime context.
        /// </summary>
        Runtime
    }
}