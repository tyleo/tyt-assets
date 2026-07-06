#nullable enable

using System.Runtime.CompilerServices;
using Tyleo.Extensions;

namespace Tyleo.Game
{
    /// <summary>
    /// Represents a color in the HSVA color space.
    /// </summary>
    public readonly struct TyHsvaColor
    {
        /// <summary>
        /// The hue component of the color, in the range [0, 1].
        /// </summary>
        public readonly float H;

        /// <summary>
        /// The saturation component of the color, in the range [0, 1].
        /// </summary>
        public readonly float S;

        /// <summary>
        /// The value component of the color, in the range [0, 1].
        /// </summary>
        public readonly float V;

        /// <summary>
        /// The alpha component of the color, in the range [0, 1].
        /// </summary>
        public readonly float A;

        public TyHsvaColor(
            float h,
            float s,
            float v,
            float a
        )
        {
            H = h;
            S = s;
            V = v;
            A = a;
        }

        /// <summary>
        /// Converts this color to an RGBA color.
        /// </summary>
        /// <returns>The RGBA representation of this color.</returns>
        public readonly TyRgbaColor ToRgba()
        {
            var h = H.Wrap01();

            if (S <= 0f)
            {
                // Gray
                return new(V, V, V, 1f);
            }

            var hf = h * 6f;
            var i = (int)hf; // 0..5 typically
            var f = hf - i;

            var p = V * (1f - S);
            var q = V * (1f - S * f);
            var t = V * (1f - S * (1f - f));

            // i can be 6 if h was 1 exactly; `WrapHue` prevents that, but keep
            // safe anyway.
            return (i % 6) switch
            {
                0 => new(V, t, p, A),
                1 => new(q, V, p, A),
                2 => new(p, V, t, A),
                3 => new(p, q, V, A),
                4 => new(t, p, V, A),
                _ => new(V, p, q, A)
            };
        }
    }
}