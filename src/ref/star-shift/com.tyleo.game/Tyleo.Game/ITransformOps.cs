#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for transforms.
    /// </summary>
    public interface ITransformOps
    {
        /// <summary>
        /// Gets the world position of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The world position of the transform.</returns>
        TyVector3 GetWorldPosition(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the world position of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="position">The new world position.</param>
        void SetWorldPosition(
            U32Id<MTransform> transformId,
            TyVector3 position
        );

        /// <summary>
        /// Gets the world rotation of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The world rotation of the transform.</returns>
        TyQuaternion GetWorldRotation(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the world rotation of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="rotation">The new world rotation.</param>
        void SetWorldRotation(
            U32Id<MTransform> transformId,
            TyQuaternion rotation
        );

        /// <summary>
        /// Gets the world scale of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The world scale of the transform.</returns>
        TyVector3 GetLossyWorldScale(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Gets the world pose of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The world pose of the transform.</returns>
        TyPose GetWorldPose(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the world position and rotation of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="position">The new world position.</param>
        /// <param name="rotation">The new world rotation.</param>
        void SetWorldPositionAndRotation(
            U32Id<MTransform> transformId,
            TyVector3 position,
            TyQuaternion rotation
        );

        /// <summary>
        /// Gets the lossy world TRS of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The lossy world TRS of the transform.</returns>
        TyTrs GetLossyWorldTrs(U32Id<MTransform> transformId);

        /// <summary>
        /// Gets the local pose of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The local pose of the transform.</returns>
        TyPose GetLocalPose(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the local pose of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="pose">The new local pose.</param>
        void SetLocalPose(
            U32Id<MTransform> transformId,
            TyPose pose
        );

        /// <summary>
        /// Gets the local position of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The local position of the transform.</returns>
        TyVector3 GetLocalPosition(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the local position of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="position">The new local position.</param>
        void SetLocalPosition(
            U32Id<MTransform> transformId,
            TyVector3 position
        );

        /// <summary>
        /// Gets the local rotation of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The local rotation of the transform.</returns>
        TyQuaternion GetLocalRotation(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the local rotation of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="rotation">The new local rotation.</param>
        void SetLocalRotation(
            U32Id<MTransform> transformId,
            TyQuaternion rotation
        );

        /// <summary>
        /// Gets the local scale of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <returns>The local scale of the transform.</returns>
        TyVector3 GetLocalScale(
            U32Id<MTransform> transformId
        );

        /// <summary>
        /// Sets the local scale of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="scale">The new local scale.</param>
        void SetLocalScale(
            U32Id<MTransform> transformId,
            TyVector3 scale
        );

        /// <summary>
        /// Sets the local position and rotation of the given transform.
        /// </summary>
        /// <param name="transformId">The id of the transform.</param>
        /// <param name="position">The new local position.</param>
        /// <param name="rotation">The new local rotation.</param>
        void SetLocalPositionAndRotation(
            U32Id<MTransform> transformId,
            TyVector3 position,
            TyQuaternion rotation
        );
    }
}