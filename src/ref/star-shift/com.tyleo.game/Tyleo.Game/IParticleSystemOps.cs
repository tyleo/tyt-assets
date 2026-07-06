#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for particle systems.
    /// </summary>
    public interface IParticleSystemOps
    {
        /// <summary>
        /// Starts emitting particles from a particle system.
        /// </summary>
        /// <param name="particleSystemId">
        /// The id of the particle system to play.
        /// </param>
        void PlayEmission(U32Id<MParticleSystem> particleSystemId);

        /// <summary>
        /// Stops emitting particles from a particle system.
        /// </summary>
        /// <param name="particleSystemId">
        /// The id of the particle system to stop.
        /// </param>
        void StopEmission(U32Id<MParticleSystem> particleSystemId);
    }
}