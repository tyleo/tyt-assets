#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.Extensions;

namespace Tyleo.Game
{
    /// <summary>
    /// A transform consisting of translation, rotation, and a scale factor.
    /// </summary>
    public readonly struct TyTrs : IEquatable<TyTrs>
    {
        /// <summary>
        /// The translation component of the transform.
        /// </summary>
        public readonly TyVector3 Translation;

        /// <summary>
        /// The rotation component of the transform.
        /// </summary>
        public readonly TyQuaternion Rotation;

        /// <summary>
        /// The scale factor of the transform.
        /// </summary>
        public readonly TyVector3 Scale;

        /// <summary>
        /// The identity transform.
        /// </summary>
        public readonly static TyTrs Identity =
            new(TyVector3.Zero, TyQuaternion.Identity, TyVector3.One);

        public TyTrs(
            TyVector3 translation,
            TyQuaternion rotation,
            TyVector3 scale
        )
        {
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        /// <summary>
        /// Converts this transform to a pose, ignoring scale.
        /// </summary>
        /// <returns>The pose representation of this transform.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TyPose ToPose() => new(Translation, Rotation);

        /// <summary>
        /// Converts this transform to a uniform scale transform.
        /// </summary>
        /// <returns>
        /// The uniform scale transform representation of this transform.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TyUniformTrs ToUniformTrs()
        {
            Scale.AssertIsPositiveUniformScale();
            return new(
                Translation,
                Rotation,
                Scale.X
            );
        }

        /// <summary>
        /// Deconstructs this transform into its components.
        /// </summary>
        /// <param name="translation">The translation component.</param>
        /// <param name="rotation">The rotation component.</param>
        /// <param name="scale">The scale component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(
            out TyVector3 translation,
            out TyQuaternion rotation,
            out TyVector3 scale
        )
        {
            translation = Translation;
            rotation = Rotation;
            scale = Scale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(in TyTrs other) => this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(int precision) =>
            $"{{ \"Translation\": {Translation.ToString(precision)}, \"Rotation\": {Rotation.ToString(precision)}, \"Scale\": {Scale.ToString(precision)} }}";

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            in TyTrs lhs,
            in TyTrs rhs
        ) =>
            lhs.Translation == rhs.Translation &&
            lhs.Rotation == rhs.Rotation &&
            lhs.Scale == rhs.Scale;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            in TyTrs lhs,
            in TyTrs rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) =>
            obj is TyTrs other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(
            Translation,
            Rotation,
            Scale
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() =>
            ToString(TyleoConst.DefaultFloatStringPrecision);

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TyTrs other) => this == other;

        #endregion IEquatable
    }
}