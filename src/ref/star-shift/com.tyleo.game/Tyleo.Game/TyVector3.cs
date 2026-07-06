#nullable enable

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tyleo.Extensions;
using Tyleo.Game.Extensions;
using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// A 3D vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 3)]
    public readonly struct TyVector3 : IEquatable<TyVector3>
    {
        /// <summary>
        /// The zero vector.
        /// </summary>
        public static readonly TyVector3 Zero = new(0, 0, 0);

        /// <summary>
        /// The one vector.
        /// </summary>
        public static readonly TyVector3 One = new(1, 1, 1);

        /// <summary>
        /// The unit vector along the negative X axis.
        /// </summary>
        public static readonly TyVector3 UnitNegativeX = FromX(-1);

        /// <summary>
        /// The unit vector along the negative Y axis.
        /// </summary>
        public static readonly TyVector3 UnitNegativeY = FromY(-1);

        /// <summary>
        /// The unit vector along the negative Z axis.
        /// </summary>
        public static readonly TyVector3 UnitNegativeZ = FromZ(-1);

        /// <summary>
        /// The unit vector along the X axis.
        /// </summary>
        public static readonly TyVector3 UnitX = FromX(1);

        /// <summary>
        /// The unit vector along the Y axis.
        /// </summary>
        public static readonly TyVector3 UnitY = FromY(1);

        /// <summary>
        /// The unit vector along the Z axis.
        /// </summary>
        public static readonly TyVector3 UnitZ = FromZ(1);

        /// <summary>
        /// The X component.
        /// </summary>
        public readonly float X;

        /// <summary>
        /// The Y component.
        /// </summary>
        public readonly float Y;

        /// <summary>
        /// The Z component.
        /// </summary>
        public readonly float Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TyVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns a vector with the specified X component and zero Y and Z
        /// components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <returns>
        /// A vector with the specified X component and zero Y and Z components.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 FromX(float x) => new(x, 0, 0);

        /// <summary>
        /// Returns a vector with the specified Y component and zero X and Z
        /// components.
        /// </summary>
        /// <param name="y">The Y component.</param>
        /// <returns>
        /// A vector with the specified Y component and zero X and Z components.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 FromY(float y) => new(0, y, 0);

        /// <summary>
        /// Returns a vector with the specified Z component and zero X and Y
        /// components.
        /// </summary>
        /// <param name="z">The Z component.</param>
        /// <returns>
        /// A vector with the specified Z component and zero X and Y components.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 FromZ(float z) => new(0, 0, z);

        /// <summary>
        /// Returns a vector with the absolute values of each component.
        /// </summary>
        /// <returns>The absolute vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 Abs() => new(X.Abs(), Y.Abs(), Z.Abs());

        /// <summary>
        /// Asserts that all components of this vector are approximately equal.
        /// </summary>
        /// <param name="name">
        /// The name of the vector (for error messages).
        /// </param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AssertComponentsApproximatelyEqual(
            string? name = null
        )
        {
            var log = GameModule.Log.Math;

            log
                .IfFalseAndDebug(X.IsApproximatelyEqual(Y))
                ?.Error(
                    $"{name ?? nameof(TyVector3)} X and Y components not approximately equal."
                );

            log
                .IfFalseAndDebug(X.IsApproximatelyEqual(Z))
                ?.Error(
                    $"{name ?? nameof(TyVector3)} X and Z components not approximately equal."
                );

            log
                .IfFalseAndDebug(Y.IsApproximatelyEqual(Z))
                ?.Error(
                    $"{name ?? nameof(TyVector3)} Y and Z components not approximately equal."
                );
        }

        /// <summary>
        /// Asserts that all components of this vector are greater than zero.
        /// </summary>
        /// <param name="name">
        /// The name of the vector (for error messages).
        /// </param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AssertComponentsGreaterThanZero(
            string? name = null
        )
        {
            var log = GameModule.Log.Math;

            log
                .IfFalseAndDebug(X > 0)
                ?.Error($"{name ?? nameof(TyVector3)} X component is not greater than zero.");

            log
                .IfFalseAndDebug(Y > 0)
                ?.Error($"{name ?? nameof(TyVector3)} Y component is not greater than zero.");

            log
                .IfFalseAndDebug(Z > 0)
                ?.Error($"{name ?? nameof(TyVector3)} Z component is not greater than zero.");
        }

        /// <summary>
        /// Asserts that this vector is a positive uniform scale.
        /// </summary>
        /// <param name="name">
        /// The name of the vector (for error messages).
        /// </param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AssertIsPositiveUniformScale(
            string? name = null
        )
        {
            AssertComponentsApproximatelyEqual(name);
            AssertComponentsGreaterThanZero(name);
        }

        /// <summary>
        /// Asserts that this vector is normalized.
        /// </summary>
        /// <param name="name">
        /// The name of the vector (for error messages).
        /// </param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AssertIsNormalized(string? name = null) => GameModule
            .Log
            .Math
            .IfFalseAndDebug((GetLengthSquared() - 1.0f).Abs() < GameConst.DefaultTolerance)
            ?.Error($"{name ?? nameof(TyVector3)} is not normalized.");

        /// <summary>
        /// Computes the component-wise multiplication of this vector with
        /// another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>
        /// The component-wise multiplication of the two vectors.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 ComponentwiseMultiply(
            TyVector3 other
        ) => new(X * other.X, Y * other.Y, Z * other.Z);

        /// <summary>
        /// Computes the cross product of this vector with another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public readonly TyVector3 Cross(TyVector3 other) => new(
            Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X
        );

        /// <summary>
        /// The length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetLength() => GetLengthSquared().SquareRoot();

        /// <summary>
        /// The squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetLengthSquared() => X.Square() + Y.Square() + Z.Square();

        /// <summary>
        /// The normalized vector.
        /// </summary>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 GetNormalized()
        {
            var length = GetLength();

            GameModule.Log.Math.IfFalseAndDebug(length > 0)?.Error(
                "Cannot normalize a zero-length vector."
            );

            return this / length;
        }

        /// <summary>
        /// Determines whether this vector is approximately equal to another.
        /// </summary>
        /// <remarks>
        /// Assumes the vectors are normalized.
        /// </remarks>
        /// <param name="other">The other vector.</param>
        /// <returns>
        /// True if the vectors are approximately equal; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsNormalizedApproximatelyEqual(
            TyVector3 other
        )
        {
            AssertIsNormalized();
            other.AssertIsNormalized();

            return Dot(other) >= 1.0f - GameConst.DefaultTolerance;
        }

        /// <summary>
        /// True if vectors point in approximately the same direction (works for non-normalized).
        /// </summary>
        /// <remarks>
        /// Returns false if either vector is near-zero length.
        /// Interprets tolerance as the same threshold you used for normalized vectors:
        /// dot(unitA, unitB) >= 1 - tolerance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsApproximatelyEqual(
            TyVector3 other
        )
        {
            // cosMin matches your normalized test threshold.
            float cosMin = 1.0f - GameConst.DefaultTolerance;

            float dot = Dot(other);
            if (dot <= 0.0f) return false; // reject opposite-ish directions early

            float lenSqA = GetLengthSquared();
            float lenSqB = other.GetLengthSquared();

            // Handle degenerate vectors (no direction).
            // Pick an epsilon appropriate for your numeric scale.
            const float kEps = GameConst.DefaultDepenetrationToleranceSquared;
            if (lenSqA <= kEps || lenSqB <= kEps) return false;

            // Compare: dot^2 >= (|a|^2)(|b|^2)(cosMin^2)
            float lhs = dot * dot;
            float rhs = lenSqA * lenSqB * (cosMin * cosMin);

            return lhs >= rhs;
        }

        /// <summary>
        /// Computes the dot product of this vector with another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Dot(TyVector3 other) =>
            X * other.X + Y * other.Y + Z * other.Z;

        /// <summary>
        /// Computes the component-wise maximum of this vector with another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The component-wise maximum of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 ComponentMaxWith(
            TyVector3 other
        ) => new(
            X.MaxWith(other.X),
            Y.MaxWith(other.Y),
            Z.MaxWith(other.Z)
        );

        /// <summary>
        /// Computes the component-wise minimum of this vector with another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The component-wise minimum of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 ComponentMinWith(
            TyVector3 other
        ) => new(
            X.MinWith(other.X),
            Y.MinWith(other.Y),
            Z.MinWith(other.Z)
        );

        /// <summary>
        /// Creates a quaternion representing a rotation around this vector.
        /// </summary>
        /// <param name="angleRadians">The angle in radians.</param>
        /// <returns>
        /// The resulting quaternion representing the rotation.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion RotationAround(float angleRadians) =>
            TyQuaternion.FromAxisAngle(this, angleRadians);

        /// <summary>
        /// Rotates this vector towards the target vector.
        /// </summary>
        /// <param name="target">The target vector.</param>
        /// <returns>The resulting quaternion representing the rotation.</returns>
        public readonly TyQuaternion RotateTowards(TyVector3 target)
        {
            AssertIsNormalized();
            target.AssertIsNormalized();

            var d = Dot(target);

            // If the vectors are almost identical: no rotation.
            if (d >= 1.0f - GameConst.DefaultTolerance)
            {
                return TyQuaternion.Identity;
            }

            // If vectors are nearly opposite: need a 180° rotation around ANY orthogonal axis.
            if (d <= -1.0f + GameConst.DefaultTolerance)
            {
                // Pick the axis most orthogonal to 'from'
                var axis =
                    X.Abs() <= Y.Abs() && X.Abs() <= Z.Abs() ?
                    UnitX :
                    Y.Abs() <= Z.Abs() ?
                    UnitY :
                    UnitZ;

                axis = Cross(axis).GetNormalized();
                return TyQuaternion.FromAxisAngle(axis, TyMath.PI); // 180°
            }

            // General case:
            var c = Cross(target);

            // A well-known stable construction:
            // q = [c.x, c.y, c.z, 1 + dot(from,to)] then normalize.
            var q = new TyQuaternion(c.X, c.Y, c.Z, 1.0f + d);
            return q.GetNormalized();
        }

        /// <summary>
        /// Converts this vector to a pure quaternion.
        /// </summary>
        /// <returns>The converted <see cref="TyQuaternion"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion ToPureQuaternion() => new(X, Y, Z, 0);

        /// <summary>
        /// Converts this vector to a single component, assuming all components
        /// are approximately equal and greater than zero.
        /// </summary>
        /// <param name="name">
        /// The name of the vector (for error messages).
        /// </param>
        /// <returns>The scale factor.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float ToScale(
            string? name = null
        )
        {
            AssertComponentsApproximatelyEqual(name);
            AssertComponentsGreaterThanZero(name);
            return X;
        }

        /// <summary>
        /// Deconstructs this vector into its components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out float x,
            out float y,
            out float z
        )
        {
            x = X;
            y = Y;
            z = Z;
        }

        public readonly string ToString(int precision) =>
            $"{{ \"X\": {X.ToString($"F{precision}")}, \"Y\": {Y.ToString($"F{precision}")}, \"Z\": {Z.ToString($"F{precision}")} }}";

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator -(TyVector3 vec) =>
            new(-vec.X, -vec.Y, -vec.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator +(TyVector3 lhs, TyVector3 rhs) =>
            new(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator -(TyVector3 lhs, TyVector3 rhs) =>
            new(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator *(TyVector3 lhs, float rhs) =>
            new(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator *(float lhs, TyVector3 rhs) =>
            new(lhs * rhs.X, lhs * rhs.Y, lhs * rhs.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator /(TyVector3 lhs, float rhs) =>
            new(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 operator /(float lhs, TyVector3 rhs) =>
            new(lhs / rhs.X, lhs / rhs.Y, lhs / rhs.Z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            TyVector3 lhs,
            TyVector3 rhs
        ) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            TyVector3 lhs,
            TyVector3 rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) =>
            obj is TyVector3 other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(
            X,
            Y,
            Z
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() => ToString(TyleoConst.DefaultFloatStringPrecision);

        #endregion Object

        #region IEquatable

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(TyVector3 other) => this == other;

        #endregion IEquatable
    }
}