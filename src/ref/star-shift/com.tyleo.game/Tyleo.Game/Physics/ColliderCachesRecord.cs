#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game.Physics
{
    /// <summary>
    /// A record of collider cache ids partitioned by shape, plus a combined
    /// cache containing every collider.
    /// </summary>
    public readonly struct ColliderCachesRecord
    {
        /// <summary>
        /// The id of the cache containing every collider.
        /// </summary>
        public readonly U32Id<MColliderCache> AllColliderCacheId;

        /// <summary>
        /// The id of the cache containing only box colliders.
        /// </summary>
        public readonly U32Id<MBoxColliderCache> BoxColliderCacheId;

        /// <summary>
        /// The id of the cache containing only capsule colliders.
        /// </summary>
        public readonly U32Id<MCapsuleColliderCache> CapsuleColliderCacheId;

        /// <summary>
        /// The id of the cache containing only sphere colliders.
        /// </summary>
        public readonly U32Id<MSphereColliderCache> SphereColliderCacheId;

        public ColliderCachesRecord(
            U32Id<MColliderCache> allColliderCacheId,
            U32Id<MBoxColliderCache> boxColliderCacheId,
            U32Id<MCapsuleColliderCache> capsuleColliderCacheId,
            U32Id<MSphereColliderCache> sphereColliderCacheId
        )
        {
            AllColliderCacheId = allColliderCacheId;
            BoxColliderCacheId = boxColliderCacheId;
            CapsuleColliderCacheId = capsuleColliderCacheId;
            SphereColliderCacheId = sphereColliderCacheId;
        }

        /// <summary>
        /// Deconstructs the collider caches record.
        /// </summary>
        public readonly void Deconstruct(
            out U32Id<MColliderCache> allColliderCacheId,
            out U32Id<MBoxColliderCache> boxColliderCacheId,
            out U32Id<MCapsuleColliderCache> capsuleColliderCacheId,
            out U32Id<MSphereColliderCache> sphereColliderCacheId
        )
        {
            allColliderCacheId = AllColliderCacheId;
            boxColliderCacheId = BoxColliderCacheId;
            capsuleColliderCacheId = CapsuleColliderCacheId;
            sphereColliderCacheId = SphereColliderCacheId;
        }

        public override string ToString() =>
            $"{{ AllColliderCacheId: {AllColliderCacheId}, BoxColliderCacheId: {BoxColliderCacheId}, CapsuleColliderCacheId: {CapsuleColliderCacheId}, SphereColliderCacheId: {SphereColliderCacheId} }}";
    }
}
