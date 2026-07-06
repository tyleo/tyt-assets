#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for mesh filters.
    /// </summary>
    public interface IMeshFilterOps
    {
        /// <summary>
        /// Replaces the mesh of a mesh filter.
        /// </summary>
        /// <param name="meshFilterId">The id of the mesh filter.</param>
        /// <param name="meshId">The id of the replacement mesh.</param>
        void ReplaceMesh(
            U32Id<MMeshFilter> meshFilterId,
            U32Id<MMesh> meshId
        );
    }
}
