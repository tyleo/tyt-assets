#nullable enable

using System.Collections.Generic;

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// A dispatcher for dynamic events. This is responsible for dispatching
    /// dynamic events to the appropriate callbacks based on the kinds of the
    /// objects involved.
    /// </summary>
    public readonly struct DoubleDispatcher
    {
        private readonly Dictionary<DoubleDispatchLookupId, IDoubleDispatchCallback> _dispatchCallbacksById;

        public DoubleDispatcher(
            Dictionary<DoubleDispatchLookupId, IDoubleDispatchCallback> dispatchCallbacksById
        )
        {
            _dispatchCallbacksById = dispatchCallbacksById;
        }

        /// <summary>
        /// Creates a new <see cref="DispatcherBuilder"/>
        /// instance that can be used to register dispatch callbacks.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="DispatcherBuilder"/>
        /// class.
        /// </returns>
        public static DispatcherBuilder Begin() =>
            new(new Dictionary<DoubleDispatchLookupId, IDoubleDispatchCallback>());

        /// <summary>
        /// Handles a dynamic event.
        /// </summary>
        /// <param name="dispatchId">
        /// The id of the dynamic event to handle.
        /// </param>
        /// <returns>
        /// True if the event was handled, false otherwise.
        /// </returns>
        public bool Dispatch(DoubleDispatchId dispatchId)
        {
            var (lookupId, objectIds) = dispatchId.Split();
            var callbackOpt = _dispatchCallbacksById.GetValueOrDefault(
                lookupId
            );

            if (callbackOpt == null) return false;
            return callbackOpt.Dispatch(objectIds);
        }
    }
}