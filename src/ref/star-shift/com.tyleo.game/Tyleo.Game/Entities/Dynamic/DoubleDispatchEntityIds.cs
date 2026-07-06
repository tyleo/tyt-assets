#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.IdSys;

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// Contains the two objects involved in a dynamic dispatch lookup.
    /// </summary>
    public readonly struct DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> :
        IEquatable<DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId>>,
        IComparable<DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId>>
        where TMLesserKindId : MDynamicEntity
        where TMGreaterKindId : MDynamicEntity
    {
        /// <summary>
        /// The identifier of the object with the lesser kind.
        /// </summary>
        public readonly U32Id<TMLesserKindId> LesserKindEntityId;

        /// <summary>
        /// The identifier of the object with the greater kind.
        /// </summary>
        public readonly U32Id<TMGreaterKindId> GreaterKindEntityId;

        public DoubleDispatchEntityIds(
            U32Id<TMLesserKindId> lesserKindEntityId,
            U32Id<TMGreaterKindId> greaterKindEntityId
        )
        {
            LesserKindEntityId = lesserKindEntityId;
            GreaterKindEntityId = greaterKindEntityId;
        }

        #region Standard

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out U32Id<TMLesserKindId> lesserKindEntityId,
            out U32Id<TMGreaterKindId> greaterKindEntityId
        )
        {
            lesserKindEntityId = LesserKindEntityId;
            greaterKindEntityId = GreaterKindEntityId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> a,
            DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> b
        ) =>
            a.LesserKindEntityId == b.LesserKindEntityId &&
            a.GreaterKindEntityId == b.GreaterKindEntityId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> a,
            DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> b
        ) => !(a == b);

        #endregion Standard

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) =>
            obj is DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> other &&
            this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override int GetHashCode() =>
            HashCode.Combine(LesserKindEntityId, GreaterKindEntityId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override string ToString() =>
            $"{{ \"LessObjectId\": {LesserKindEntityId}, \"GreaterObjectId\": {GreaterKindEntityId} }}";

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(
            DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> other
        ) => this == other;

        #endregion IEquatable

        #region IComparable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(DoubleDispatchEntityIds<TMLesserKindId, TMGreaterKindId> other)
        {
            var kindComparison = LesserKindEntityId.CompareTo(other.LesserKindEntityId);
            if (kindComparison != 0) return kindComparison;
            return GreaterKindEntityId.CompareTo(other.GreaterKindEntityId);
        }

        #endregion IComparable
    }
}