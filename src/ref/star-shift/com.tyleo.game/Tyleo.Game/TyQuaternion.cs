#nullable enable

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tyleo.Extensions;
using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// A quaternion.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 4)]
    public readonly struct TyQuaternion : IEquatable<TyQuaternion>
    {
        public static readonly TyQuaternion Identity = new(0, 0, 0, 1);

        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float W;

        public TyQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Creates a quaternion from a normalized axis and an angle.
        /// </summary>
        /// <param name="axis">The normalized axis to rotate around.</param>
        /// <param name="angleRadians">The angle to rotate by (in radians).</param>
        /// <returns>The resulting quaternion.</returns>
        public static TyQuaternion FromAxisAngle(
            TyVector3 axis,
            float angleRadians
        )
        {
            axis.AssertIsNormalized();

            var halfAngle = angleRadians * 0.5f;
            var sin = halfAngle.SinOfRadians();
            var cosine = halfAngle.CosOfRadians();

            return new TyQuaternion(
                axis.X * sin,
                axis.Y * sin,
                axis.Z * sin,
                cosine
            );
        }

        /// <summary>
        /// Creates a quaternion from basis vectors.
        /// </summary>
        /// <param name="right">The right vector.</param>
        /// <param name="up">The up vector.</param>
        /// <param name="forwards">The forwards vector.</param>
        /// <returns>The resulting quaternion.</returns>
        public static TyQuaternion FromBasisVectors(
            TyVector3 right,
            TyVector3 up,
            TyVector3 forwards
        )
        {
            right.AssertIsNormalized(nameof(right));
            up.AssertIsNormalized(nameof(up));
            forwards.AssertIsNormalized(nameof(forwards));

            // Assumes the basis vectors are orthonormal.
            var trace = right.X + up.Y + forwards.Z;
            if (trace > 0.0f)
            {
                var s = (trace + 1.0f).SquareRoot() * 2.0f;
                return new TyQuaternion(
                    (up.Z - forwards.Y) / s,
                    (forwards.X - right.Z) / s,
                    (right.Y - up.X) / s,
                    0.25f * s
                );
            }
            else if (right.X > up.Y && right.X > forwards.Z)
            {
                var s = (1.0f + right.X - up.Y - forwards.Z).SquareRoot() * 2.0f;
                return new TyQuaternion(
                    0.25f * s,
                    (right.Y + up.X) / s,
                    (forwards.X + right.Z) / s,
                    (up.Z - forwards.Y) / s
                );
            }
            else if (up.Y > forwards.Z)
            {
                var s = (1.0f + up.Y - right.X - forwards.Z).SquareRoot() * 2.0f;
                return new TyQuaternion(
                    (right.Y + up.X) / s,
                    0.25f * s,
                    (up.Z + forwards.Y) / s,
                    (forwards.X - right.Z) / s
                );
            }
            else
            {
                var s = (1.0f + forwards.Z - right.X - up.Y).SquareRoot() * 2.0f;
                return new TyQuaternion(
                    (forwards.X + right.Z) / s,
                    (up.Z + forwards.Y) / s,
                    0.25f * s,
                    (right.Y - up.X) / s
                );
            }
        }

        /// <summary>
        /// Creates a quaternion from right and forward basis vectors in a
        /// left-handed coordinate system.
        /// </summary>
        /// <param name="right">The right vector.</param>
        /// <param name="forward">The forward vector.</param>
        /// <returns>The resulting quaternion.</returns>
        public static TyQuaternion FromRightForward(
            TyVector3 right,
            TyVector3 forward
        )
        {
            right.AssertIsNormalized();
            forward.AssertIsNormalized();

            var up = forward.Cross(right).GetNormalized();
            return FromBasisVectors(right, up, forward);
        }

        /// <summary>
        /// Creates a quaternion from right and up basis vectors in a
        /// left-handed coordinate system.
        /// </summary>
        /// <param name="right">The right vector.</param>
        /// <param name="up">The up vector.</param>
        /// <returns>The resulting quaternion.</returns>
        public static TyQuaternion FromRightUp(
            TyVector3 right,
            TyVector3 up
        )
        {
            right.AssertIsNormalized();
            up.AssertIsNormalized();

            var forward = right.Cross(up).GetNormalized();
            return FromBasisVectors(right, up, forward);
        }

        /// <summary>
        /// Asserts that this quaternion is in canonical form (W >= 0).
        /// </summary>
        /// <param name="name">
        /// The name of the quaternion (for error messages).
        /// </param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AssertIsCanonicalized(string? name = null) =>
            GameModule
                .Log
                .Math
                .IfFalseAndDebug(W >= 0.0f)
                ?.Error($"{name ?? nameof(TyQuaternion)} is not canonicalized.");

        /// <summary>
        /// Asserts that this quaternion is normalized.
        /// </summary>
        /// <param name="name">
        /// The name of the quaternion (for error messages).
        /// </param>
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void AssertIsNormalized(string? name = null) => GameModule
            .Log
            .Math
            .IfFalseAndDebug((GetLengthSquared() - 1.0f).Abs() < GameConst.DefaultTolerance)
            ?.Error($"{name ?? nameof(TyQuaternion)} is not normalized.");

        /// <summary>
        /// Gets the conjugate of this quaternion.
        /// </summary>
        /// <remarks>
        /// The conjugate of a quaternion is equivalent to its inverse if the
        /// quaternion is normalized.
        /// </remarks>
        /// <returns>The conjugate quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion GetConjugate() => new(-X, -Y, -Z, W);

        /// <summary>
        /// Returns the canonical form of this quaternion.
        /// </summary>
        /// <returns>
        /// The canonicalized quaternion (with non-negative W component).
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion GetCanonicalized() =>
            W >= 0.0f ? this : -this;

        /// <summary>
        /// Gets the inverse of this quaternion.
        /// </summary>
        /// <returns>The inverse quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion GetInverse()
        {
            var inverseSquaredLength = GetInverseSquaredLength();
            // The inverse of a quaternion is its conjugate divided by its
            // squared length.
            return new TyQuaternion(
                -X * inverseSquaredLength,
                -Y * inverseSquaredLength,
                -Z * inverseSquaredLength,
                W * inverseSquaredLength
            );
        }

        /// <summary>
        /// Gets the inverse squared length of this quaternion.
        /// </summary>
        /// <returns>The inverse squared length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetInverseSquaredLength()
        {
            var lengthSq = GetLengthSquared();
            GameModule
                .Log
                .Math
                .IfFalseAndDebug(lengthSq > GameConst.DefaultToleranceSquared)
                ?.Error("Cannot compute the inverse squared length of a quaternion with zero length.");
            return 1.0f / lengthSq;
        }

        /// <summary>
        /// Gets the length of this quaternion.
        /// </summary>
        /// <returns>The length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetLength() =>
            GetLengthSquared().SquareRoot();

        /// <summary>
        /// Gets the squared length of this quaternion.
        /// </summary>
        /// <returns>The squared length.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float GetLengthSquared() =>
            X.Square() + Y.Square() + Z.Square() + W.Square();

        /// <summary>
        /// Gets the normalized version of this quaternion.
        /// </summary>
        /// <returns>The normalized quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion GetNormalized()
        {
            var length = GetLength();

            GameModule.Log.Math.IfFalseAndDebug(length > 0)?.Error(
                "Cannot normalize a quaternion with zero length."
            );

            return new TyQuaternion(
                X / length,
                Y / length,
                Z / length,
                W / length
            );
        }

        /// <summary>
        /// Gets the vector part of this quaternion.
        /// </summary>
        /// <returns>The vector part.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 GetVector3Part() => new(X, Y, Z);

        /// <summary>
        /// Computes the dot product of this quaternion with another.
        /// </summary>
        /// <param name="other">The other quaternion.</param>
        /// <returns>The dot product of the two quaternions.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float Dot(TyQuaternion other) =>
            X * other.X + Y * other.Y + Z * other.Z + W * other.W;

        /// <summary>
        /// Rotates a vector by this quaternion.
        /// </summary>
        /// <remarks>
        /// Assumes the quaternion is normalized.
        /// </remarks>
        /// <param name="vector">The vector to rotate.</param>
        /// <returns>The rotated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 Rotate(TyVector3 vector)
        {
            AssertIsNormalized();

            var vectorQuat = vector.ToPureQuaternion();

            // Assume the quaternion is normalized.
            // Rotate the vector using the formula: // q * v * conjugate(q)
            var resultQuat = this * vectorQuat * GetConjugate();

            return resultQuat.GetVector3Part();
        }

        /// <summary>
        /// Rotates this quaternion around the given axis by the specified
        /// angle.
        /// </summary>
        /// <param name="axis">The axis to rotate around.</param>
        /// <param name="angleRadians">
        /// The angle to rotate by (in radians).
        /// </param>
        /// <returns>The rotated quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyQuaternion RotateAroundAxis(
            TyVector3 axis,
            float angleRadians
        )
        {
            AssertIsNormalized();
            axis.AssertIsNormalized();

            var rotationQuat = FromAxisAngle(axis, angleRadians);
            var resultQuat = rotationQuat * this;

            return resultQuat;
        }

        /// <summary>
        /// Rotates extents by this quaternion, using absolute values to ensure
        /// the extents remain positive.
        /// </summary>
        /// <param name="extents">The vector representing the extents.</param>
        /// <returns>The rotated extents.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector3 RotateExtentsAbs(TyVector3 extents)
        {
            AssertIsNormalized();

            var matrix = ToMatrix4x4();

            var newX =
                matrix.M00.Abs() * extents.X +
                matrix.M01.Abs() * extents.Y +
                matrix.M02.Abs() * extents.Z;

            var newY =
                matrix.M10.Abs() * extents.X +
                matrix.M11.Abs() * extents.Y +
                matrix.M12.Abs() * extents.Z;

            var newZ =
                matrix.M20.Abs() * extents.X +
                matrix.M21.Abs() * extents.Y +
                matrix.M22.Abs() * extents.Z;

            return new(newX, newY, newZ);
        }

        /// <summary>
        /// Determines whether this quaternion is approximately equal to
        /// another.
        /// </summary>
        /// <remarks>
        /// Assumes the quaternions are normalized.
        /// </remarks>
        /// <param name="other">The other quaternion.</param>
        /// <returns>
        /// True if the quaternions are approximately equal; otherwise, false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool IsApproximatelyEqual(
            TyQuaternion other
        )
        {
            AssertIsNormalized();
            other.AssertIsNormalized();

            return Dot(other).Abs() >= 1.0f - GameConst.DefaultTolerance;
        }

        /// <summary>
        /// Spherically interpolates between two normalized quaternions.
        /// </summary>
        /// <param name="to">Target rotation (must be normalized).</param>
        /// <param name="t">
        /// Interpolation factor in [0, 1].
        /// 0 returns this, 1 returns <paramref name="to"/>.
        /// </param>
        /// <returns>The interpolated quaternion.</returns>
        public readonly TyQuaternion SlerpTowards(
            TyQuaternion to,
            float t
        )
        {
            AssertIsNormalized();
            to.AssertIsNormalized();

            var dot = Dot(to);

            // Take shortest path (q and -q represent the same rotation)
            if (dot < 0.0f)
            {
                to = -to;
                dot = -dot;
            }

            // If the quaternions are very close, fall back to normalized LERP
            if (dot > GameConst.SlerpDotThreshold)
            {
                // Linear interpolation
                var result = this * (1.0f - t) + to * t;
                return result.GetNormalized();
            }

            // Standard SLERP
            // angle between from and to
            dot = dot.Clamp(-1.0f, 1.0f);
            var theta0 = dot.AcosRadians();
            var theta = theta0 * t;

            var sinTheta0 = theta0.SinOfRadians();
            var sinTheta = theta.SinOfRadians();

            // Hard guard anyway (belt + suspenders)
            // If sinTheta0 is extremely small, fallback to nlerp
            if (sinTheta0 < GameConst.DefaultTolerance)
            {
                // Linear interpolation
                var lerp = this * (1.0f - t) + (to * t);
                return lerp.GetNormalized();
            }

            var s0 = theta.CosOfRadians() - dot * sinTheta / sinTheta0;
            var s1 = sinTheta / sinTheta0;

            return this * s0 + to * s1;
        }

        /// <summary>
        /// Converts this quaternion to a rotation matrix.
        /// </summary>
        /// <returns>The rotation matrix.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TyMatrix4x4 ToMatrix4x4()
        {
            var num = X * 2f;
            var num2 = Y * 2f;
            var num3 = Z * 2f;
            var num4 = X * num;
            var num5 = Y * num2;
            var num6 = Z * num3;
            var num7 = X * num2;
            var num8 = X * num3;
            var num9 = Y * num3;
            var num10 = W * num;
            var num11 = W * num2;
            var num12 = W * num3;

            return new(
                1f - (num5 + num6),
                num7 + num12,
                num8 - num11,
                0f,

                num7 - num12,
                1f - (num4 + num6),
                num9 + num10,
                0f,

                num8 + num11,
                num9 - num10,
                1f - (num4 + num5),
                0f,

                0f,
                0f,
                0f,
                1f
            );

        }

        /// <summary>
        /// Deconstructs this quaternion into its components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out float x,
            out float y,
            out float z,
            out float w
        )
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }

        public readonly string ToString(int precision) =>
            $"{{ \"X\": {X.ToString($"F{precision}")}, \"Y\": {Y.ToString($"F{precision}")}, \"Z\": {Z.ToString($"F{precision}")}, \"W\": {W.ToString($"F{precision}")} }}";

        #region Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyQuaternion operator -(
            TyQuaternion self
        ) => new(-self.X, -self.Y, -self.Z, -self.W);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyQuaternion operator +(
            TyQuaternion lhs,
            TyQuaternion rhs
        ) => new(
            lhs.X + rhs.X,
            lhs.Y + rhs.Y,
            lhs.Z + rhs.Z,
            lhs.W + rhs.W
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyQuaternion operator -(
            TyQuaternion lhs,
            TyQuaternion rhs
        ) => new(
            lhs.X - rhs.X,
            lhs.Y - rhs.Y,
            lhs.Z - rhs.Z,
            lhs.W - rhs.W
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyQuaternion operator *(
            TyQuaternion lhs,
            float rhs
        ) => new(
            lhs.X * rhs,
            lhs.Y * rhs,
            lhs.Z * rhs,
            lhs.W * rhs
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyQuaternion operator *(
            float lhs,
            TyQuaternion rhs
        ) => new(
            lhs * rhs.X,
            lhs * rhs.Y,
            lhs * rhs.Z,
            lhs * rhs.W
        );

        public static TyQuaternion operator *(
            TyQuaternion lhs,
            TyQuaternion rhs
        ) => new(
            lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y,
            lhs.W * rhs.Y - lhs.X * rhs.Z + lhs.Y * rhs.W + lhs.Z * rhs.X,
            lhs.W * rhs.Z + lhs.X * rhs.Y - lhs.Y * rhs.X + lhs.Z * rhs.W,
            lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(
            TyQuaternion lhs,
            TyQuaternion rhs
        ) =>
            lhs.X == rhs.X &&
            lhs.Y == rhs.Y &&
            lhs.Z == rhs.Z &&
            lhs.W == rhs.W;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(
            TyQuaternion lhs,
            TyQuaternion rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly bool Equals(object? obj) =>
            obj is TyQuaternion other && this == other;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => HashCode.Combine(
            X,
            Y,
            Z,
            W
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly string ToString() =>
            ToString(TyleoConst.DefaultFloatStringPrecision);

        #endregion Object

        #region IEquatable

        public readonly bool Equals(TyQuaternion other) => this == other;

        #endregion IEquatable
    }
}