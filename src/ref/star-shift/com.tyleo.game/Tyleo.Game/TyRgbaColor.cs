#nullable enable

using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Tyleo.Game
{
    /// <summary>
    /// A color with RGBA components, each in the range [0, 1].
    /// </summary>
    public readonly struct TyRgbaColor
    {
        /// <summary>
        /// The red component of the color, in the range [0, 1].
        /// </summary>
        public readonly float R;

        /// <summary>
        /// The green component of the color, in the range [0, 1].
        /// </summary>
        public readonly float G;

        /// <summary>
        /// The blue component of the color, in the range [0, 1].
        /// </summary>
        public readonly float B;

        /// <summary>
        /// The alpha component of the color, in the range [0, 1].
        /// </summary>
        public readonly float A;

        public TyRgbaColor(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Converts this color to an HSVA color.
        /// </summary>
        /// <returns>The HSVA representation of this color.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TyHsvaColor ToHsva()
        {

            float max = R > G ? (R > B ? R : B) : (G > B ? G : B);
            float min = R < G ? (R < B ? R : B) : (G < B ? G : B);
            float delta = max - min;

            float h = 0f;
            float s = (max <= 0f) ? 0f : (delta / max);
            float v = max;

            if (delta > 0f)
            {
                if (max == R)
                {
                    h = (G - B) / delta;
                    if (h < 0f) h += 6f;
                }
                else if (max == G)
                {
                    h = 2f + (B - R) / delta;
                }
                else // max == B
                {
                    h = 4f + (R - G) / delta;
                }

                h /= 6f; // to [0,1)
            }

            return new TyHsvaColor(h, s, v, A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyRgbaColor operator *(TyRgbaColor self, float scalar) =>
            new(
                self.R * scalar,
                self.G * scalar,
                self.B * scalar,
                self.A * scalar
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TyRgbaColor operator *(float scalar, TyRgbaColor self) =>
            new(
                self.R * scalar,
                self.G * scalar,
                self.B * scalar,
                self.A * scalar
            );
    }
}