#nullable enable

namespace Tyleo.Game.Entities
{
    /// <summary>
    /// An interface for primary objects that can register themselves.
    /// </summary>
    /// <typeparam name="TWorld">
    /// The type of the world that the primary object can register to.
    /// </typeparam>
    public interface ISelfRegisteringEntity<TWorld>
    {
        /// <summary>
        /// Registers this primary object to the given world. This is usually
        /// called for static objects during startup.
        /// </summary>
        /// <param name="world">
        /// The world to register this primary object to.
        /// </param>
        void RegisterToWorld(TWorld world);

        /// <summary>
        /// Initializes any borrowed ids that this primary object has. This is
        /// called after all primary objects have been registered to the world,
        /// so their ids exist.
        /// </summary>
        /// <param name="world">
        /// The world the object registered with.
        /// </param>
        void InitializeBorrowedIds(TWorld world)
        {
            // By default, do nothing. This is for primary objects that don't
            // have any borrowed ids, so they don't need to initialize them.
        }

        /// <summary>
        /// Disposes of this primary object. This is called when the world is
        /// being disposed, so the primary object can clean up any resources it
        /// has. By default, this does nothing, but primary objects that have
        /// resources to clean up can override this method to do so.
        /// </summary>
        /// <param name="world">
        /// The world the object registered with.
        /// </param>
        void Dispose(TWorld world) { }
    }
}