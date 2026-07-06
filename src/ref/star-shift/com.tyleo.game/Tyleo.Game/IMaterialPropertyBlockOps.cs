#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for material property blocks.
    /// </summary>
    public interface IMaterialPropertyBlockOps
    {
        /// <summary>
        /// Sets a color property of a material property block.
        /// </summary>
        /// <param name="materialPropertyBlockId">
        /// The id of the material property block.
        /// </param>
        /// <param name="propertyId">The id of the property.</param>
        /// <param name="color">The color to set.</param>
        void SetColor(
            U32Id<MMaterialPropertyBlock> materialPropertyBlockId,
            U32Id<MMaterialColorProperty> propertyId,
            TyRgbaColor color
        );

        /// <summary>
        /// Sets a float property of a material property block.
        /// </summary>
        /// <param name="materialPropertyBlockId">
        /// The id of the material property block.
        /// </param>
        /// <param name="propertyId">The id of the property.</param>
        /// <param name="value">The float value to set.</param>
        void SetFloat(
            U32Id<MMaterialPropertyBlock> materialPropertyBlockId,
            U32Id<MMaterialFloatProperty> propertyId,
            float value
        );

        /// <summary>
        /// Reads the material property block of a renderer.
        /// </summary>
        /// <param name="materialPropertyBlockId">
        /// The id of the material property block.
        /// </param>
        /// <param name="rendererId">The id of the renderer.</param>
        void ReadFrom(
            U32Id<MMaterialPropertyBlock> materialPropertyBlockId,
            U32Id<MRenderer> rendererId
        );

        /// <summary>
        /// Writes the material property block of a renderer.
        /// </summary>
        /// <param name="materialPropertyBlockId">
        /// The id of the material property block.
        /// </param>
        /// <param name="rendererId">The id of the renderer.</param>
        void WriteTo(
            U32Id<MMaterialPropertyBlock> materialPropertyBlockId,
            U32Id<MRenderer> rendererId
        );
    }
}