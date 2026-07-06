#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.IdSys;
using Tyleo.Logging;

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// An identifier for a dynamic dispatch lookup, which is used to look up
    /// the appropriate dynamic dispatch implementation for a pair of primary
    /// object kinds.
    /// </summary>
    public readonly struct DoubleDispatchLookupId :
        IEquatable<DoubleDispatchLookupId>,
        IComparable<DoubleDispatchLookupId>
    {
        /// <summary>
        /// <para>
        /// The identifier of the primary object kind with the smaller
        /// identifier.
        /// </para>
        /// <para>
        /// This is used to ensure a consistent order when comparing or hashing
        /// pairs of identifiers, so that the order of the identifiers does not
        /// affect the result.
        /// </para>
        /// </summary>
        public readonly U32Id<MDynamicEntityKind> LesserKindId;

        /// <summary>
        /// <para>
        /// The identifier of the primary object kind with the larger
        /// identifier.
        /// </para>
        /// <para>
        /// This is used to ensure a consistent order when comparing or hashing
        /// pairs of identifiers, so that the order of the identifiers does not
        /// affect the result.
        /// </para>
        /// </summary>
        public readonly U32Id<MDynamicEntityKind> GreaterKindId;

        public DoubleDispatchLookupId(
            U32Id<MDynamicEntityKind> lesserKindId,
            U32Id<MDynamicEntityKind> greaterKindId
        )
        {
            var debugAssertions = GameModule.Log.DebugAssertions.IfDebug();
            if (debugAssertions != null)
            {
                var (less, greater) = lesserKindId.OrderWith(greaterKindId);
                if (less != lesserKindId || greater != greaterKindId)
                {
                    debugAssertions.Error(
                        $"`{nameof(DoubleDispatchLookupId)}` must be constructed with the smaller kind id first. " +
                        $"Received {{ \"lessKindId\": {lesserKindId}, \"greaterKindId\": {greaterKindId} }}."
                    );
                }
            }

            LesserKindId = lesserKindId;
            GreaterKindId = greaterKindId;
        }

        #region Standard

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out U32Id<MDynamicEntityKind> kindIdA,
            out U32Id<MDynamicEntityKind> kindIdB
        )
        {
            kindIdA = LesserKindId;
            kindIdB = GreaterKindId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            DoubleDispatchLookupId a,
            DoubleDispatchLookupId b
        ) => a.LesserKindId == b.LesserKindId && a.GreaterKindId == b.GreaterKindId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            DoubleDispatchLookupId a,
            DoubleDispatchLookupId b
        ) => !(a == b);

        #endregion Standard

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) =>
            obj is DoubleDispatchLookupId other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override int GetHashCode() =>
            HashCode.Combine(LesserKindId, GreaterKindId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override string ToString() =>
            $"{{ \"LessKindId\": {LesserKindId}, \"GreaterKindId\": {GreaterKindId} }}";

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(DoubleDispatchLookupId other) =>
            this == other;

        #endregion IEquatable

        #region IComparable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(DoubleDispatchLookupId other)
        {
            var kindComparison = LesserKindId.CompareTo(other.LesserKindId);
            if (kindComparison != 0) return kindComparison;
            return GreaterKindId.CompareTo(other.GreaterKindId);
        }

        #endregion IComparable
    }
}