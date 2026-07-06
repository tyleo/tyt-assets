#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for colliders.
    /// </summary>
    public interface IColliderOps
    {
        /// <summary>
        /// Calculates a new position that resolves penetrations for a set of
        /// colliders.
        /// </summary>
        /// <param name="desiredWorldPosition">
        /// The desired world position of the root object.
        /// </param>
        /// <param name="desiredWorldRotation">
        /// The desired world rotation of the root object.
        /// </param>
        /// <param name="desiredWorldScale">
        /// The desired world scale of the root object.
        /// </param>
        /// <param name="collidersId">
        /// The id of the colliders to check.
        /// </param>
        /// <param name="cachedBroadPhaseBoundsInRoot">
        /// Axis-aligned bounds to use for broad-phase collision, in the root
        /// object's local space. This should encapsulate all colliders.
        /// </param>
        /// <param name="cachedColliderInRootPoses">
        /// The poses of the colliders relative in the root object's local
        /// space.
        /// </param>
        /// <param name="overlapBufferCacheId">
        /// The id of a buffer to store overlaps in.
        /// </param>
        /// <param name="maxIterations">
        /// The maximum number of iterations.
        /// </param>
        /// <param name="extraSkinWidth">
        /// The extra skin width.
        /// </param>
        /// <param name="layerMask">
        /// The layer mask for the broad-phase overlap query.
        /// </param>
        /// <returns>
        /// The calculated depenetration position.
        /// </returns>
        TyVector3 CalculateDepenetrationPosition(
            TyVector3 desiredWorldPosition,
            TyQuaternion desiredWorldRotation,
            float desiredWorldScale,
            U32Id<MColliderCache> collidersId,
            in TyBounds cachedBroadPhaseBoundsInRoot,
            TyPose[] cachedColliderInRootPoses,
            U32Id<MColliderCache> overlapBufferCacheId,
            int maxIterations = GameConst.DefaultDepenetrationStepCount,
            float extraSkinWidth = GameConst.DefaultSkinWidth,
            int layerMask = ~0
        );

        /// <summary>
        /// Calculates a new position that does not penetrate any colliders by
        /// step-and-slide sweeping the given box, capsule, and sphere
        /// colliders from <paramref name="initialWorldPosition"/> toward
        /// <paramref name="desiredWorldPosition"/>. Each iteration sweeps from
        /// the latest non-penetrating position (rather than the rigid body's
        /// current position), so multi-step moves see a consistent start
        /// pose.
        /// </summary>
        /// <param name="initialWorldPosition">
        /// The starting world position for the sweep.
        /// </param>
        /// <param name="desiredWorldPosition">
        /// The desired world position to sweep toward.
        /// </param>
        /// <param name="worldRotation">
        /// The world rotation to apply to the cached collider poses while
        /// sweeping.
        /// </param>
        /// <param name="worldScale">
        /// The uniform world scale to apply to the cached collider poses
        /// while sweeping.
        /// </param>
        /// <param name="selfCollidersId">
        /// The id of the cache containing every collider that belongs to the
        /// moving object. Hits against these colliders are ignored.
        /// </param>
        /// <param name="boxCollidersId">
        /// The id of the cache containing the moving object's box colliders
        /// to sweep.
        /// </param>
        /// <param name="cachedBoxColliderInRootPoses">
        /// The poses of the box colliders in the root object's local space,
        /// indexed in lockstep with the box collider cache.
        /// </param>
        /// <param name="capsuleCollidersId">
        /// The id of the cache containing the moving object's capsule
        /// colliders to sweep.
        /// </param>
        /// <param name="cachedCapsuleColliderInRootPoses">
        /// The poses of the capsule colliders in the root object's local
        /// space, indexed in lockstep with the capsule collider cache.
        /// </param>
        /// <param name="sphereCollidersId">
        /// The id of the cache containing the moving object's sphere
        /// colliders to sweep.
        /// </param>
        /// <param name="cachedSphereColliderInRootPoses">
        /// The poses of the sphere colliders in the root object's local
        /// space, indexed in lockstep with the sphere collider cache.
        /// </param>
        /// <param name="sweepHitBufferCacheId">
        /// The id of a raycast-hit buffer to scratch into during each sweep.
        /// </param>
        /// <param name="skinWidth">
        /// The skin width to use when checking for collisions.
        /// </param>
        /// <param name="maxSteps">
        /// The maximum number of move-and-slide steps to take.
        /// </param>
        /// <param name="layerMask">
        /// The layer mask for the sweep tests. Hits on excluded layers are
        /// ignored.
        /// </param>
        TyVector3 CalculateNonPenetratingPosition(
            TyVector3 initialWorldPosition,
            TyVector3 desiredWorldPosition,
            TyQuaternion worldRotation,
            float worldScale,
            U32Id<MColliderCache> selfCollidersId,
            U32Id<MBoxColliderCache> boxCollidersId,
            TyPose[] cachedBoxColliderInRootPoses,
            U32Id<MCapsuleColliderCache> capsuleCollidersId,
            TyPose[] cachedCapsuleColliderInRootPoses,
            U32Id<MSphereColliderCache> sphereCollidersId,
            TyPose[] cachedSphereColliderInRootPoses,
            U32Id<MRaycastHitCache> sweepHitBufferCacheId,
            float skinWidth = GameConst.DefaultSkinWidth,
            int maxSteps = GameConst.DefaultDepenetrationStepCount,
            int layerMask = ~0
        );
    }
}