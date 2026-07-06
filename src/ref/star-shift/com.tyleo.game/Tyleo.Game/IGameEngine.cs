#nullable enable

namespace Tyleo.Game
{
    /// <summary>
    /// Represents the game engine.
    /// </summary>
    public interface IGameEngine
    {
        /// <inheritdoc cref="IColliderOps"/>
        IColliderOps ColliderOps { get; }

        /// <inheritdoc cref="IHierarchyNodeOps"/>
        IHierarchyNodeOps HierarchyNodeOps { get; }

        /// <inheritdoc cref="IInputActionOps"/>
        IInputActionOps InputActionOps { get; }

        /// <inheritdoc cref="IMaterialPropertyBlockOps"/>
        IMaterialPropertyBlockOps MaterialPropertyBlockOps { get; }

        /// <inheritdoc cref="IMeshFilterOps"/>
        IMeshFilterOps MeshFilterOps { get; }

        /// <inheritdoc cref="IParticleSystemOps"/>
        IParticleSystemOps ParticleSystemOps { get; }

        /// <inheritdoc cref="IRigidBodyOps"/>
        IRigidBodyOps RigidBodyOps { get; }

        /// <inheritdoc cref="ITransformOps"/>
        ITransformOps TransformOps { get; }
    }
}
