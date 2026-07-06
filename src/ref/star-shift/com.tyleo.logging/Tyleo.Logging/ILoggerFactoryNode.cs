#nullable enable

namespace Tyleo.Logging
{
    /// <summary>
    /// Factory interface for building a hierarchical logging structure.
    /// Each node can create loggers and child nodes, forming a tree rooted
    /// at a single top-level node.
    /// </summary>
    public interface ILoggerFactoryNode
    {
        /// <summary>
        /// Creates a child logger under this node.
        /// </summary>
        /// <param name="loggerName">
        /// The name of the child logger to create.
        /// </param>
        ILogger CreateChildLogger(string loggerName);

        /// <summary>
        /// Creates a child filter node under this node.
        /// </summary>
        /// <param name="filterNodeName">
        /// The name of the child filter node to create.
        /// </param>
        ILoggerFactoryNode CreateChildFilterNode(string filterNodeName);
    }
}