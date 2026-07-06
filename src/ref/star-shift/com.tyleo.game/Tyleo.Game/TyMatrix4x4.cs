#nullable enable

namespace Tyleo.Game
{
    public readonly struct TyMatrix4x4
    {
        public readonly float M00;
        public readonly float M10;
        public readonly float M20;
        public readonly float M30;
        public readonly float M01;
        public readonly float M11;
        public readonly float M21;
        public readonly float M31;
        public readonly float M02;
        public readonly float M12;
        public readonly float M22;
        public readonly float M32;
        public readonly float M03;
        public readonly float M13;
        public readonly float M23;
        public readonly float M33;

        public TyMatrix4x4(
            float m00, float m10, float m20, float m30,
            float m01, float m11, float m21, float m31,
            float m02, float m12, float m22, float m32,
            float m03, float m13, float m23, float m33
        )
        {
            M00 = m00;
            M10 = m10;
            M20 = m20;
            M30 = m30;
            M01 = m01;
            M11 = m11;
            M21 = m21;
            M31 = m31;
            M02 = m02;
            M12 = m12;
            M22 = m22;
            M32 = m32;
            M03 = m03;
            M13 = m13;
            M23 = m23;
            M33 = m33;
        }
    }
}