#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game.Extensions
{
    public static class IMaterialPropertyBlockOpsExt
    {
        /// <summary>
        /// Replaces the color of a material property block on a renderer with
        /// the given color.
        /// </summary>
        /// <param name="materialPropertyBlockOps">
        /// The material property block operations.
        /// </param>
        /// <param name="materialPropertyBlockId">
        /// The id of the material property block.
        /// </param>
        /// <param name="rendererId">The id of the renderer.</param>
        /// <param name="propertyId">The id of the color property.</param>
        /// <param name="color">The new color.</param>
        public static void ReplaceColor(
            this IMaterialPropertyBlockOps materialPropertyBlockOps,
            U32Id<MMaterialPropertyBlock> materialPropertyBlockId,
            U32Id<MRenderer> rendererId,
            U32Id<MMaterialColorProperty> propertyId,
            TyRgbaColor color
        )
        {
            materialPropertyBlockOps.ReadFrom(materialPropertyBlockId, rendererId);
            materialPropertyBlockOps.SetColor(materialPropertyBlockId, propertyId, color);
            materialPropertyBlockOps.WriteTo(materialPropertyBlockId, rendererId);
        }

        /// <summary>
        /// Replaces the float value of a material property block on a renderer
        /// with the given value.
        /// </summary>
        /// <param name="materialPropertyBlockOps">
        /// The material property block operations.
        /// </param>
        /// <param name="materialPropertyBlockId">
        /// The id of the material property block.
        /// </param>
        /// <param name="rendererId">The id of the renderer.</param>
        /// <param name="propertyId">The id of the float property.</param>
        /// <param name="value">The new float value.</param>
        public static void ReplaceFloat(
            this IMaterialPropertyBlockOps materialPropertyBlockOps,
            U32Id<MMaterialPropertyBlock> materialPropertyBlockId,
            U32Id<MRenderer> rendererId,
            U32Id<MMaterialFloatProperty> propertyId,
            float value
        )
        {
            materialPropertyBlockOps.ReadFrom(materialPropertyBlockId, rendererId);
            materialPropertyBlockOps.SetFloat(materialPropertyBlockId, propertyId, value);
            materialPropertyBlockOps.WriteTo(materialPropertyBlockId, rendererId);
        }
    }
}