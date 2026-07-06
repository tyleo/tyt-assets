#nullable enable

using System;
using System.Runtime.CompilerServices;
using Tyleo.Extensions;

namespace Tyleo.Game.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Single"/> type.
    /// </summary>
    public static class FloatExt
    {
        /// <summary>
        /// Determines whether the specified float is approximately equal to
        /// another float, within a small epsilon range.
        /// </summary>
        /// <param name="self">The first float to compare.</param>
        /// <param name="other">The second float to compare.</param>
        /// <returns>
        /// <c>true</c> if the floats are approximately equal; otherwise,
        /// <c>false</c>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximatelyEqual(
            this float self,
            float other
        ) => (self - other).Abs() < GameConst.DefaultTolerance;

        /// <summary>
        /// Linearly interpolates between two <see cref="TyVector3"/> values.
        /// </summary>
        /// <param name="self">
        /// The interpolation factor, typically in the range [0, 1].
        /// </param>
        /// <param name="from">The starting vector.</param>
        /// <param name="to">The target vector.</param>
        /// <returns>The interpolated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 LerpVector3(
            this float self,
            TyVector3 from,
            TyVector3 to
        ) => from + (to - from) * self;

        /// <summary>
        /// Linearly interpolates between two <see cref="TyHsvaColor"/> values,
        /// taking into account the circular nature of the hue component.
        /// </summary>
        /// <param name="self">
        /// The interpolation factor, typically in the range [0, 1].
        /// </param>
        /// <param name="from">The starting color.</param>
        /// <param name="to">The target color.</param>
        /// <returns>The interpolated color.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyHsvaColor LerpHsvaColor(
            this float self,
            TyHsvaColor from,
            TyHsvaColor to
        )
        {
            // Hue is circular. Interpolate on shortest arc.
            var h0 = from.H;
            var h1 = to.H;
            var dh = h1 - h0;

            // Wrap into [-0.5, 0.5] so we take the shortest path around the
            // circle.
            if (dh > 0.5f)
            {
                dh -= 1f;
            }
            else if (dh < -0.5f)
            {
                dh += 1f;
            }

            var h = (h0 + dh * self).Wrap01();
            var s = from.S + (to.S - from.S) * self;
            var v = from.V + (to.V - from.V) * self;
            var a = from.A + (to.A - from.A) * self;

            return new(h, s, v, a);
        }

        /// <summary>
        /// Evaluates a uniform Catmull-Rom spline position at parameter
        /// <paramref name="self"/> using four control points.
        /// </summary>
        /// <param name="self">
        /// The interpolation parameter t, typically in the range [0, 1].
        /// </param>
        /// <param name="p0">The control point before the segment start.</param>
        /// <param name="p1">The segment start point.</param>
        /// <param name="p2">The segment end point.</param>
        /// <param name="p3">The control point after the segment end.</param>
        /// <returns>The position on the spline at parameter t.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 CatmullRomPosition(
            this float self,
            TyVector3 p0,
            TyVector3 p1,
            TyVector3 p2,
            TyVector3 p3
        )
        {
            var t = self;
            var t2 = t * t;
            var t3 = t2 * t;

            return 0.5f * (
                p1 * 2f +
                (p2 - p0) * t +
                (p0 * 2f - p1 * 5f + p2 * 4f - p3) * t2 +
                (p1 * 3f - p0 - p2 * 3f + p3) * t3
            );
        }

        /// <summary>
        /// Evaluates the tangent (first derivative) of a uniform Catmull-Rom
        /// spline at parameter <paramref name="self"/> using four control
        /// points.
        /// </summary>
        /// <param name="self">
        /// The interpolation parameter t, typically in the range [0, 1].
        /// </param>
        /// <param name="p0">The control point before the segment start.</param>
        /// <param name="p1">The segment start point.</param>
        /// <param name="p2">The segment end point.</param>
        /// <param name="p3">The control point after the segment end.</param>
        /// <returns>The tangent of the spline at parameter t.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 CatmullRomTangent(
            this float self,
            TyVector3 p0,
            TyVector3 p1,
            TyVector3 p2,
            TyVector3 p3
        )
        {
            var t = self;
            var t2 = t * t;

            return 0.5f * (
                (p2 - p0) +
                (p0 * 4f - p1 * 10f + p2 * 8f - p3 * 2f) * t +
                (p1 * 9f - p0 * 3f - p2 * 9f + p3 * 3f) * t2
            );
        }

        /// <summary>
        /// Splats the float value into a <see cref="TyVector3"/>,
        /// setting all components to the same value.
        /// </summary>
        /// <param name="self">The float value to splat.</param>
        /// <returns>
        /// A <see cref="TyVector3"/> with all components set to the float value.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyVector3 SplatToVector3(
            this float self
        ) => new(self, self, self);
    }
}