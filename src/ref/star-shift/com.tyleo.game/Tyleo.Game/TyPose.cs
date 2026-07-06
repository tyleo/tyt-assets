#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.Collections;

namespace Tyleo.Game
{
    /// <summary>
    /// A pose consisting of a position and rotation.
    /// </summary>
    public readonly struct TyPose : IEquatable<TyPose>
    {
        /// <summary>
        /// The position component of the pose.
        /// </summary>
        public readonly TyVector3 Position;

        /// <summary>
        /// The rotation component of the pose.
        /// </summary>
        public readonly TyQuaternion Rotation;

        /// <summary>
        /// The identity pose.
        /// </summary>
        public readonly static TyPose Identity =
            new(TyVector3.Zero, TyQuaternion.Identity);

        public TyPose(TyVector3 position, TyQuaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        /// <summary>
        /// Deconstructs this pose into its components.
        /// </summary>
        /// <param name="position">The position component.</param>
        /// <param name="rotation">The rotation component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(
            out TyVector3 position,
            out TyQuaternion rotation
        )
        {
            position = Position;
            rotation = Rotation;
        }

        /// <summary>
        /// Calculates the pose of <paramref name="targetWorldPose"/> in the
        /// local space of this pose.
        /// </summary>
        /// <param name="targetWorldPose">
        /// The target pose in world-space.
        /// </param>
        /// <returns>
        /// The pose of <paramref name="targetWorldPose"/> in the local space of
        /// this pose.
        /// </returns>
        public readonly TyPose CalculateRelativePose(in TyPose targetWorldPose)
        {
            var inverseRotation = Rotation.GetInverse();

            var relativePosition = inverseRotation.Rotate(targetWorldPose.Position - Position);
            var relativeRotation = inverseRotation * targetWorldPose.Rotation;
            return new(relativePosition, relativeRotation);
        }

        /// <summary>
        /// Calculates the poses of a set of target world poses in the local
        /// space of this pose.
        /// </summary>
        /// <typeparam name="TEnumerator">
        /// The enumerator type for the target world poses.
        /// </typeparam>
        /// <param name="uniformTrsEnumerable">
        /// The enumerable of target world poses.
        /// </param>
        /// <returns>The enumerable of relative poses.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ValueEnumerable<TyPose, SelectWithStateEnumerator<TyPose, TyPose, TEnumerator, TyPose>> SelectRelativePose<TEnumerator>(
            ValueEnumerable<TyPose, TEnumerator> uniformPoseEnumerable
        ) where TEnumerator : struct, IValueEnumerator<TyPose>
        {
            return uniformPoseEnumerable.SelectWithState(
                this,
                (self, targetWorldPose) => self.CalculateRelativePose(targetWorldPose)
            );
        }

        /// <summary>
        /// Transforms an axis-aligned bounding box by this pose, growing it
        /// conservatively.
        /// </summary>
        /// <param name="aabb">
        /// The axis-aligned bounding box to transform.
        /// </param>
        /// <returns>The transformed axis-aligned bounding box.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyBounds TransformAabbConservative(in TyBounds aabb)
        {
            var center = Position + Rotation.Rotate(aabb.Center);
            var extents = Rotation.RotateExtentsAbs(aabb.Extents);
            return new(center, extents);
        }

        /// <summary>
        /// Creates a uniform TRS transform from this pose with the given scale.
        /// </summary>
        /// <param name="scale">The uniform scale factor.</param>
        /// <returns>The uniform TRS transform.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyUniformTrs WithUniformScale(float scale) =>
            new(Position, Rotation, scale);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(in TyPose other) => this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(int precision) =>
            $"{{ \"Position\": {Position.ToString(precision)}, \"Rotation\": {Rotation.ToString(precision)} }}";

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            in TyPose lhs,
            in TyPose rhs
        ) =>
            lhs.Position == rhs.Position &&
            lhs.Rotation == rhs.Rotation;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            in TyPose lhs,
            in TyPose rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) =>
            obj is TyPose other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(
            Position,
            Rotation
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() =>
            ToString(TyleoConst.DefaultFloatStringPrecision);

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TyPose other) => this == other;

        #endregion IEquatable
    }
}