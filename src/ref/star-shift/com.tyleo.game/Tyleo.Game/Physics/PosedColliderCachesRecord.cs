#nullable enable

using Tyleo.Collections;
using Tyleo.IdSys;

namespace Tyleo.Game.Physics
{
    /// <summary>
    /// A record of collider cache ids paired with their poses in root space
    /// and the combined bounds of every collider in root space.
    /// </summary>
    public readonly struct PosedColliderCachesRecord
    {
        /// <summary>
        /// The id of the cache containing every collider.
        /// </summary>
        public readonly U32Id<MColliderCache> AllColliderCacheId;

        /// <summary>
        /// The poses of every collider in root space, indexed in lockstep
        /// with the all-colliders cache.
        /// </summary>
        public readonly TyPose[] AllPosesInRoot;

        /// <summary>
        /// The id of the cache containing only box colliders.
        /// </summary>
        public readonly U32Id<MBoxColliderCache> BoxColliderCacheId;

        /// <summary>
        /// The poses of the box colliders in root space, indexed in lockstep
        /// with the box collider cache.
        /// </summary>
        public readonly TyPose[] BoxPosesInRoot;

        /// <summary>
        /// The id of the cache containing only capsule colliders.
        /// </summary>
        public readonly U32Id<MCapsuleColliderCache> CapsuleColliderCacheId;

        /// <summary>
        /// The poses of the capsule colliders in root space, indexed in
        /// lockstep with the capsule collider cache.
        /// </summary>
        public readonly TyPose[] CapsulePosesInRoot;

        /// <summary>
        /// The combined bounds of every collider in root space.
        /// </summary>
        public readonly TyBounds CombinedBoundsInRoot;

        /// <summary>
        /// The id of the cache containing only sphere colliders.
        /// </summary>
        public readonly U32Id<MSphereColliderCache> SphereColliderCacheId;

        /// <summary>
        /// The poses of the sphere colliders in root space, indexed in
        /// lockstep with the sphere collider cache.
        /// </summary>
        public readonly TyPose[] SpherePosesInRoot;

        public PosedColliderCachesRecord(
            U32Id<MColliderCache> allColliderCacheId,
            TyPose[] allPosesInRoot,
            U32Id<MBoxColliderCache> boxColliderCacheId,
            TyPose[] boxPosesInRoot,
            U32Id<MCapsuleColliderCache> capsuleColliderCacheId,
            TyPose[] capsulePosesInRoot,
            TyBounds combinedBoundsInRoot,
            U32Id<MSphereColliderCache> sphereColliderCacheId,
            TyPose[] spherePosesInRoot
        )
        {
            AllColliderCacheId = allColliderCacheId;
            AllPosesInRoot = allPosesInRoot;
            BoxColliderCacheId = boxColliderCacheId;
            BoxPosesInRoot = boxPosesInRoot;
            CapsuleColliderCacheId = capsuleColliderCacheId;
            CapsulePosesInRoot = capsulePosesInRoot;
            CombinedBoundsInRoot = combinedBoundsInRoot;
            SphereColliderCacheId = sphereColliderCacheId;
            SpherePosesInRoot = spherePosesInRoot;
        }

        /// <summary>
        /// Deconstructs the posed collider caches record.
        /// </summary>
        public readonly void Deconstruct(
            out U32Id<MColliderCache> allColliderCacheId,
            out TyPose[] allPosesInRoot,
            out U32Id<MBoxColliderCache> boxColliderCacheId,
            out TyPose[] boxPosesInRoot,
            out U32Id<MCapsuleColliderCache> capsuleColliderCacheId,
            out TyPose[] capsulePosesInRoot,
            out TyBounds combinedBoundsInRoot,
            out U32Id<MSphereColliderCache> sphereColliderCacheId,
            out TyPose[] spherePosesInRoot
        )
        {
            allColliderCacheId = AllColliderCacheId;
            allPosesInRoot = AllPosesInRoot;
            boxColliderCacheId = BoxColliderCacheId;
            boxPosesInRoot = BoxPosesInRoot;
            capsuleColliderCacheId = CapsuleColliderCacheId;
            capsulePosesInRoot = CapsulePosesInRoot;
            combinedBoundsInRoot = CombinedBoundsInRoot;
            sphereColliderCacheId = SphereColliderCacheId;
            spherePosesInRoot = SpherePosesInRoot;
        }

        public readonly string ToString(int precision) =>
            $"{{ AllColliderCacheId: {AllColliderCacheId}, AllPosesInRoot: {AllPosesInRoot.AsValueEnumerable().ToJsonString()}, BoxColliderCacheId: {BoxColliderCacheId}, BoxPosesInRoot: {BoxPosesInRoot.AsValueEnumerable().ToJsonString()}, CapsuleColliderCacheId: {CapsuleColliderCacheId}, CapsulePosesInRoot: {CapsulePosesInRoot.AsValueEnumerable().ToJsonString()}, CombinedBoundsInRoot: {CombinedBoundsInRoot.ToString(precision)}, SphereColliderCacheId: {SphereColliderCacheId}, SpherePosesInRoot: {SpherePosesInRoot.AsValueEnumerable().ToJsonString()} }}";

        public override string ToString() => ToString(TyleoConst.DefaultFloatStringPrecision);
    }
}
