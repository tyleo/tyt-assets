#nullable enable

using System;
using System.Runtime.CompilerServices;

namespace Tyleo.Game
{
    /// <summary>
    /// A structure representing an axis-aligned bounding box.
    /// </summary>
    public readonly struct TyBounds
    {
        /// <summary>
        /// A bounds with zero center and zero extents.
        /// </summary>
        public static readonly TyBounds Zero = new(
            TyVector3.Zero,
            TyVector3.Zero
        );

        /// <summary>
        /// The center of the bounds.
        /// </summary>
        public readonly TyVector3 Center;

        /// <summary>
        /// The extents of the bounds.
        /// </summary>
        public readonly TyVector3 Extents;

        public TyBounds(TyVector3 center, TyVector3 extents)
        {
            Center = center;
            Extents = extents;
        }

        /// <summary>
        /// Gets the maximum point of the bounds.
        /// </summary>
        /// <returns> The maximum point of the bounds. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 GetMax() => Center + Extents;

        /// <summary>
        /// Gets the minimum point of the bounds.
        /// </summary>
        /// <returns> The minimum point of the bounds. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 GetMin() => Center - Extents;

        /// <summary>
        /// Encapsulates this bounds with another bounds, returning a new bounds
        /// that contains both.
        /// </summary>
        /// <param name="other">The other bounds to encapsulate with.</param>
        /// <returns>The new encapsulated bounds.</returns>
        public readonly TyBounds Encapsulate(in TyBounds other)
        {
            var min = GetMin().ComponentMinWith(other.GetMin());
            var max = GetMax().ComponentMaxWith(other.GetMax());

            var newCenter = (min + max) * 0.5f;
            var newExtents = (max - min) * 0.5f;

            return new(newCenter, newExtents);
        }

        /// <summary>
        /// Scales the bounds by the given value, returning a new bounds.
        /// </summary>
        /// <param name="value">The scale factor.</param>
        /// <returns>The scaled bounds.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyBounds Scale(float value) => new(
            Center * value,
            Extents * value
        );

        /// <summary>
        /// Deconstructs this bounds into its components.
        /// </summary>
        /// <param name="center">The center component.</param>
        /// <param name="extents">The extents component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out TyVector3 center,
            out TyVector3 extents
        )
        {
            center = Center;
            extents = Extents;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(in TyBounds other) => this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(int precision) =>
            $"{{ \"Center\": {Center.ToString(precision)}, \"Extents\": {Extents.ToString(precision)} }}";


        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            in TyBounds lhs,
            in TyBounds rhs
        ) =>
            lhs.Center == rhs.Center &&
            lhs.Extents == rhs.Extents;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            in TyBounds lhs,
            in TyBounds rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) =>
            obj is TyBounds other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(
            Center,
            Extents
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() =>
            ToString(TyleoConst.DefaultFloatStringPrecision);

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TyBounds other) => this == other;

        #endregion IEquatable
    }
}