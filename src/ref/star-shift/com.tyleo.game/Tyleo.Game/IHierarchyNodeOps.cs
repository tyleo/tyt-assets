#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for hierarchy nodes.
    /// </summary>
    public interface IHierarchyNodeOps
    {
        /// <summary>
        /// Gets whether a hierarchy node is active.
        /// </summary>
        /// <param name="hierarchyNodeId">
        /// The id of the hierarchy node.
        /// </param>
        /// <returns>
        /// Whether the hierarchy node is active.
        /// </returns>
        bool GetIsActive(U32Id<MHierarchyNode> hierarchyNodeId);

        /// <summary>
        /// Sets whether a hierarchy node is active.
        /// </summary>
        /// <param name="hierarchyNodeId">
        /// The id of the hierarchy node.
        /// </param>
        /// <param name="isActive">
        /// Whether the hierarchy node should be active.
        /// </param>
        void SetIsActive(U32Id<MHierarchyNode> hierarchyNodeId, bool isActive);

        /// <summary>
        /// Sets the parent of a hierarchy node.
        /// </summary>
        /// <param name="childId">
        /// The id of the child hierarchy node.
        /// </param>
        /// <param name="parentId">
        /// The id of the parent hierarchy node.
        /// </param>
        void SetParent(
            U32Id<MHierarchyNode> childId,
            U32Id<MHierarchyNode> parentId
        );
    }
}
