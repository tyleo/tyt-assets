#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Tyleo.Extensions;
using Tyleo.Logging;

namespace Tyleo.Game
{
    /// <summary>
    /// A 3D vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(float) * 2)]
    public readonly struct TyVector2 : IEquatable<TyVector2>
    {
        /// <summary>
        /// The zero vector.
        /// </summary>
        public static readonly TyVector2 Zero = new(0, 0);

        /// <summary>
        /// The unit vector along each axis.
        /// </summary>
        public static readonly TyVector2 UnitX = new(1, 0);

        /// <summary>
        /// The unit vector along the Y axis.
        /// </summary>
        public static readonly TyVector2 UnitY = new(0, 1);

        /// <summary>
        /// The X component.
        /// </summary>
        public readonly float X;

        /// <summary>
        /// The Y component.
        /// </summary>
        public readonly float Y;

        public TyVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Computes the cross product of this vector with another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public readonly TyVector2 Cross(TyVector2 other) => new(
            X * other.Y - Y * other.X,
            Y * other.X - X * other.Y
        );

        /// <summary>
        /// Computes the dot product of this vector with another.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public readonly float Dot(TyVector2 other) => X * other.X + Y * other.Y;

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
        public readonly float GetLengthSquared() => X.Square() + Y.Square();

        /// <summary>
        /// Returns a normalized version of this vector.
        /// </summary>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyVector2 GetNormalized()
        {
            var length = GetLength();

            GameModule.Log.Math.IfFalseAndDebug(length > 0)?.Error(
                "Cannot normalize a zero-length vector."
            );

            return this / length;
        }

        /// <summary>
        /// Deconstructs this vector into its components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Deconstruct(
            out float x,
            out float y
        )
        {
            x = X;
            y = Y;
        }

        public readonly string ToString(int precision) =>
            $"{{ \"X\": {X.ToString($"F{precision}")}, \"Y\": {Y.ToString($"F{precision}")} }}";

        /// <summary>
        /// Converts this vector to a <see cref="TyVector3"/> with a Z value of
        /// 0.
        /// </summary>
        /// <returns>The converted <see cref="TyVector3"/>.</returns>
        public readonly TyVector3 ToVector3() => new(X, Y, 0);

        #region Operators

        public static TyVector2 operator -(TyVector2 vec) =>
            new(-vec.X, -vec.Y);

        public static TyVector2 operator +(TyVector2 lhs, TyVector2 rhs) =>
            new(lhs.X + rhs.X, lhs.Y + rhs.Y);

        public static TyVector2 operator -(TyVector2 lhs, TyVector2 rhs) =>
            new(lhs.X - rhs.X, lhs.Y - rhs.Y);

        public static TyVector2 operator *(TyVector2 lhs, float rhs) =>
            new(lhs.X * rhs, lhs.Y * rhs);

        public static TyVector2 operator /(TyVector2 lhs, float rhs) =>
            new(lhs.X / rhs, lhs.Y / rhs);

        public static bool operator ==(
            TyVector2 lhs,
            TyVector2 rhs
        ) => lhs.X == rhs.X && lhs.Y == rhs.Y;

        public static bool operator !=(
            TyVector2 lhs,
            TyVector2 rhs
        ) => !(lhs == rhs);

        #endregion Operators

        #region Object

        public override readonly bool Equals(object? obj) =>
            obj is TyVector2 other && this == other;

        public override readonly int GetHashCode() => HashCode.Combine(
            X,
            Y
        );

        public override readonly string ToString() => ToString(TyleoConst.DefaultFloatStringPrecision);

        #endregion Object

        #region IEquatable

        public readonly bool Equals(TyVector2 other) => this == other;

        #endregion IEquatable
    }
}