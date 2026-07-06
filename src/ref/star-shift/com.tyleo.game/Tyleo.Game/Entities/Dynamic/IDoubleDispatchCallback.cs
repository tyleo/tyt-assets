#nullable enable

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// An interface that represents the callback for a dynamic dispatch event.
    /// </summary>
    public interface IDoubleDispatchCallback
    {
        /// <summary>
        /// Callback for when a dynamic event occurs between two objects. The
        /// object ids are provided, and the callback can perform any necessary
        /// logic based on the event.
        /// </summary>
        /// <param name="objectIds">
        /// The ids of the objects involved in the dynamic event.
        /// </param>
        /// <returns>
        /// True if the event was handled, false otherwise.
        /// </returns>
        bool Dispatch(DoubleDispatchEntityIds<MDynamicEntity, MDynamicEntity> objectIds);
    }
}