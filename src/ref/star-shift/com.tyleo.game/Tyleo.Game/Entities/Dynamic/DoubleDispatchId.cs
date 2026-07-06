#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.Logging;

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// An identifier for a dynamic dispatch, which is used to look up the
    /// appropriate dynamic dispatch implementation for a pair of primary
    /// objects.
    /// </summary>
    public readonly struct DoubleDispatchId :
        IEquatable<DoubleDispatchId>,
        IComparable<DoubleDispatchId>
    {
        /// <summary>
        /// <para>
        /// The identifier of the primary object with the smaller identifier.
        /// </para>
        /// <para>
        /// This is used to ensure a consistent order when comparing or hashing
        /// pairs of identifiers, so that the order of the identifiers does not
        /// affect the result.
        /// </para>
        /// </summary>
        public readonly DynamicEntityId LessId;

        /// <summary>
        /// <para>
        /// The identifier of the primary object with the larger identifier.
        /// </para>
        /// <para>
        /// This is used to ensure a consistent order when comparing or hashing
        /// pairs of identifiers, so that the order of the identifiers does not
        /// affect the result.
        /// </para>
        /// </summary>
        public readonly DynamicEntityId GreaterId;

        public DoubleDispatchId(
            DynamicEntityId lessId,
            DynamicEntityId greaterId
        )
        {
            var debugAssertions = GameModule.Log.DebugAssertions.IfDebug();
            if (debugAssertions != null)
            {
                var (debugLessId, debugGreaterId) = lessId.OrderWith(greaterId);
                if (debugLessId != lessId || debugGreaterId != greaterId)
                {
                    debugAssertions.Error(
                        $"`{nameof(DoubleDispatchId)}` must be constructed with the smaller kind id first. " +
                        $"Received {{ \"lessKindId\": {lessId}, \"greaterKindId\": {greaterId} }}."
                    );
                }
            }

            LessId = lessId;
            GreaterId = greaterId;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly (
            DoubleDispatchLookupId LookupId,
            DoubleDispatchEntityIds<MDynamicEntity, MDynamicEntity> EntityIds
        ) Split() => (
            new(LessId.KindId, GreaterId.KindId),
            new(LessId.EntityId, GreaterId.EntityId)
        );

        #region Standard

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out DynamicEntityId lessId,
            out DynamicEntityId greaterId
        )
        {
            lessId = LessId;
            greaterId = GreaterId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            DoubleDispatchId a,
            DoubleDispatchId b
        ) => a.LessId == b.LessId && a.GreaterId == b.GreaterId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            DoubleDispatchId a,
            DoubleDispatchId b
        ) => !(a == b);

        #endregion Standard

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override bool Equals(object? obj) =>
            obj is DoubleDispatchId other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override int GetHashCode() =>
            HashCode.Combine(LessId, GreaterId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly override string ToString() =>
            $"{{ \"LessId\": {LessId}, \"GreaterId\": {GreaterId} }}";

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(DoubleDispatchId other) =>
            this == other;

        #endregion IEquatable

        #region IComparable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(DoubleDispatchId other)
        {
            var lessComparison = LessId.CompareTo(other.LessId);
            if (lessComparison != 0) return lessComparison;
            return GreaterId.CompareTo(other.GreaterId);
        }

        #endregion IComparable
    }
}