#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.IdSys;

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// A unique identifier for a primary object which includes the kind of the
    /// object. This is used for dynamic primary objects which can have
    /// different kinds, so that the kind of the object can be determined from
    /// the identifier.
    /// </summary>
    public readonly struct DynamicEntityId :
        IEquatable<DynamicEntityId>,
        IComparable<DynamicEntityId>
    {
        /// <summary>
        /// The kind of the primary object.
        /// </summary>
        public readonly U32Id<MDynamicEntityKind> KindId;

        /// <summary>
        /// The identifier of the primary object within its kind.
        /// </summary>
        public readonly U32Id<MDynamicEntity> EntityId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DynamicEntityId(
            U32Id<MDynamicEntityKind> kindId,
            U32Id<MDynamicEntity> entityId
        )
        {
            KindId = kindId;
            EntityId = entityId;
        }

        /// <summary>
        /// Determines the order of two dynamic primary object identifiers for
        /// the purpose of dynamic dispatch. This is used to ensure a consistent
        /// order when comparing or hashing.
        /// </summary>
        /// <param name="other">The other identifier to compare against.</param>
        /// <returns>
        /// A dynamic dispatch identifier representing the order of the two
        /// identifiers.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly DoubleDispatchId DoubleDispatchWith(
            DynamicEntityId other
        )
        {
            var (less, greater) = OrderWith(other);
            return new(less, greater);
        }

        /// <summary>
        /// Determines the order of two dynamic primary object identifiers for
        /// the purpose of consistent ordering. This is used to ensure a
        /// consistent order when comparing or hashing.
        /// </summary> <param name="other">
        /// The other identifier to compare against.
        /// </param>
        /// <returns>>
        /// A tuple containing the two identifiers in a consistent order.
        /// </returns>
        public (DynamicEntityId Less, DynamicEntityId Greater) OrderWith(
            DynamicEntityId other
        )
        {
            var comparison = KindId.CompareTo(other.KindId);
            return comparison switch
            {
                < 0 => new(this, other),
                > 0 => new(other, this),
                _ => EntityId.CompareTo(other.EntityId) < 0
                    ? new(this, other)
                    : new(other, this)
            };
        }

        #region Standard

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out U32Id<MDynamicEntityKind> kindId,
            out U32Id<MDynamicEntity> id
        )
        {
            kindId = KindId;
            id = EntityId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            DynamicEntityId a,
            DynamicEntityId b
        ) => a.KindId == b.KindId && a.EntityId == b.EntityId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            DynamicEntityId a,
            DynamicEntityId b
        ) => !(a == b);

        #endregion Standard

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) =>
            obj is DynamicEntityId other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override int GetHashCode() =>
            HashCode.Combine(KindId, EntityId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override string ToString() =>
            $"{{ \"KindId\": {KindId}, \"Id\": {EntityId} }}";

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(DynamicEntityId other) =>
            this == other;

        #endregion IEquatable

        #region IComparable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(DynamicEntityId other)
        {
            var kindComparison = KindId.CompareTo(other.KindId);
            if (kindComparison != 0) return kindComparison;
            return EntityId.CompareTo(other.EntityId);
        }

        #endregion IComparable
    }
}