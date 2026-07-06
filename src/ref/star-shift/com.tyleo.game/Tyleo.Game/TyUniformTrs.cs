#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.Collections;
using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// A transform consisting of translation, rotation, and a uniform scale factor.
    /// </summary>
    public readonly struct TyUniformTrs : IEquatable<TyUniformTrs>
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
        /// The uniform scale factor of the transform.
        /// </summary>
        public readonly float Scale;

        /// <summary>
        /// The identity transform.
        /// </summary>
        public readonly static TyUniformTrs Identity =
            new(TyVector3.Zero, TyQuaternion.Identity, 1.0f);

        public TyUniformTrs(
            TyVector3 translation,
            TyQuaternion rotation,
            float scale
        )
        {
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        /// <summary>
        /// Calculates the transform of <paramref name="targetWorldTransform"/>
        /// in the local space of this transform.
        /// </summary>
        /// <param name="targetWorldTransform">
        /// The target world transform to calculate the relative transform for.
        /// </param>
        /// <returns>The relative transform.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the scale is zero.
        /// </exception>
        public readonly TyUniformTrs CalculateRelativeTrs(in TyUniformTrs targetWorldTransform)
        {
            GameModule.Log.Math.IfFalseAndDebug(Scale > 0)?.Error(
                "Calculating relative transform with non-positive scale will produce invalid results."
            );

            var inverseRotation = Rotation.GetInverse();
            var inverseScale = 1.0f / Scale;

            var deltaWorldTranslation = targetWorldTransform.Translation - Translation;
            var relativeTranslation = inverseRotation.Rotate(
                deltaWorldTranslation * inverseScale
            );

            var relativeRotation = inverseRotation * targetWorldTransform.Rotation;

            var relativeScale = targetWorldTransform.Scale * inverseScale;

            return new(relativeTranslation, relativeRotation, relativeScale);
        }

        /// <summary>
        /// Calculates the transforms of a set of target world transforms in the
        /// local space of this transform.
        /// </summary>
        /// <typeparam name="TEnumerator">
        /// The enumerator type for the target world transforms.
        /// </typeparam>
        /// <param name="uniformTrsEnumerable">
        /// The enumerable of target world transforms.
        /// </param>
        /// <returns>>The enumerable of relative transforms.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ValueEnumerable<TyUniformTrs, SelectWithStateEnumerator<TyUniformTrs, TyUniformTrs, TEnumerator, TyUniformTrs>> SelectRelativeTrs<TEnumerator>(
            ValueEnumerable<TyUniformTrs, TEnumerator> uniformTrsEnumerable
        ) where TEnumerator : struct, IValueEnumerator<TyUniformTrs>
        {
            return uniformTrsEnumerable.SelectWithState(
                this,
                (self, targetWorldTransform) => self.CalculateRelativeTrs(targetWorldTransform)
            );
        }

        /// <summary>
        /// Converts this trs to a pose, ignoring scale.
        /// </summary>
        /// <returns>The pose representation of this transform.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TyPose ToPose() => new(Translation, Rotation);

        /// <summary>
        /// Transforms an axis-aligned bounding box by this trs, growing it
        /// conservatively.
        /// </summary>
        /// <param name="aabb">
        /// The axis-aligned bounding box to transform.
        /// </param>
        /// <returns>The transformed axis-aligned bounding box.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyBounds TransformAabbConservative(in TyBounds aabb)
        {
            GameModule.Log.Math.IfFalseAndDebug(Scale > 0)?.Error(
                "Transforming AABB with non-positive scale will produce invalid results."
            );

            var center =
                Translation +
                Rotation.Rotate(aabb.Center * Scale);

            var extents =
                Rotation.RotateExtentsAbs(aabb.Extents * Scale);

            return new(center, extents);
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
            out float scale
        )
        {
            translation = Translation;
            rotation = Rotation;
            scale = Scale;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(in TyUniformTrs other) => this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(int precision) =>
            $"{{ \"Translation\": {Translation.ToString(precision)}, \"Rotation\": {Rotation.ToString(precision)}, \"Scale\": {Scale.ToString($"F{precision}")} }}";

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            in TyUniformTrs lhs,
            in TyUniformTrs rhs
        ) =>
            lhs.Translation == rhs.Translation &&
            lhs.Rotation == rhs.Rotation &&
            lhs.Scale == rhs.Scale;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            in TyUniformTrs lhs,
            in TyUniformTrs rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) =>
            obj is TyUniformTrs other && this == other;

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
        public bool Equals(TyUniformTrs other) => this == other;

        #endregion IEquatable
    }
}