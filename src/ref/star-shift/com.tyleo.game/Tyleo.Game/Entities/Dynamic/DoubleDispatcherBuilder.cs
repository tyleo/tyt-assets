#nullable enable

using System;
using System.Collections.Generic;
using Tyleo.Collections;
using Tyleo.IdSys;

namespace Tyleo.Game.Entities.Dynamic
{
    /// <summary>
    /// A builder for <see cref="DoubleDispatcher"/> that allows for
    /// registering dynamic event callbacks. This is used to build a
    /// <see cref="DoubleDispatcher"/> instance by registering callbacks for
    /// different combinations of primary object kinds. Once all callbacks have
    /// been registered, call <see cref="End"/> to get the final
    /// <see cref="DoubleDispatcher"/> instance.
    /// </summary>
    public readonly struct DispatcherBuilder
    {
        private readonly Dictionary<DoubleDispatchLookupId, IDoubleDispatchCallback> _dispatchCallbacksById;

        internal DispatcherBuilder(
            Dictionary<DoubleDispatchLookupId, IDoubleDispatchCallback> dispatchCallbacksById
        )
        {
            _dispatchCallbacksById = dispatchCallbacksById;
        }

        /// <summary>
        /// Builds the <see cref="DoubleDispatcher"/> instance with the
        /// registered callbacks.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="DoubleDispatcher"/> class.
        /// </returns>
        public readonly DoubleDispatcher End() => new(_dispatchCallbacksById);

        /// <summary>
        /// Registers a dynamic event callback for a specific combination of
        /// primary object kinds. The primary object kinds are identified by
        /// their ids, and the callback is provided as a delegate that takes the
        /// object ids of the objects involved in the dynamic event.
        /// </summary>
        /// <typeparam name="TMA">
        /// The type of the first primary object.
        /// </typeparam>
        /// <typeparam name="TMB">
        /// The type of the second primary object.
        /// </typeparam>
        /// <param name="lesserKindId">
        /// The id of the first primary object kind.
        /// </param>
        /// <param name="greaterKindId">
        /// The id of the second primary object kind.
        /// </param>
        /// <param name="action">
        /// The callback to invoke when the dynamic event occurs.
        /// </param>
        public readonly void RegisterAction<TMA, TMB>(
            U32Id<MDynamicEntityKind> lesserKindId,
            U32Id<MDynamicEntityKind> greaterKindId,
            Action<DoubleDispatchEntityIds<TMA, TMB>> action
        )
            where TMA : MDynamicEntity
            where TMB : MDynamicEntity
        {
            var lookupId = new DoubleDispatchLookupId(lesserKindId, greaterKindId);
            var callbackWrapper = new ActionDoubleDispatchCallback<TMA, TMB>(action);
            _dispatchCallbacksById.Add(lookupId, callbackWrapper);
        }

        private sealed class ActionDoubleDispatchCallback<TMA, TMB> :
            IDoubleDispatchCallback
            where TMA : MDynamicEntity
            where TMB : MDynamicEntity
        {
            private readonly Action<DoubleDispatchEntityIds<TMA, TMB>> _callback;

            public ActionDoubleDispatchCallback(
                Action<DoubleDispatchEntityIds<TMA, TMB>> callback
            )
            {
                _callback = callback;
            }

            public bool Dispatch(
                DoubleDispatchEntityIds<MDynamicEntity, MDynamicEntity> objectIds
            )
            {
                _callback(new(
                    objectIds.LesserKindEntityId.Downcast<TMA>(),
                    objectIds.GreaterKindEntityId.Downcast<TMB>()
                ));
                return true;
            }
        }

        /// <summary>
        /// Registers a dynamic event list for a specific combination of primary
        /// object kinds. The primary object kinds are identified by their ids,
        /// and the list is provided as an out parameter that will be populated
        /// with the object ids of the objects involved in the dynamic event
        /// when it occurs.
        /// </summary>
        /// <typeparam name="TMA">
        /// The type of the first dynamic entity.
        /// </typeparam>
        /// <typeparam name="TMB">
        /// The type of the second dynamic entity.
        /// </typeparam>
        /// <param name="lesserKindId">
        /// The id of the lesser kind dynamic entity.
        /// </param>
        /// <param name="greaterKindId">
        /// The id of the greater kind dynamic entity.
        /// </param>
        /// <returns>
        /// A list of object ids for the dynamic event.
        /// </returns>
        public readonly UniqueList<DoubleDispatchEntityIds<TMA, TMB>> RegisterList<TMA, TMB>(
            U32Id<MDynamicEntityKind> lesserKindId,
            U32Id<MDynamicEntityKind> greaterKindId
        )
            where TMA : MDynamicEntity
            where TMB : MDynamicEntity
        {
            var objectIds = new UniqueList<DoubleDispatchEntityIds<TMA, TMB>>();
            var lookupId = new DoubleDispatchLookupId(lesserKindId, greaterKindId);
            var callback = new ListDoubleDispatchCallback<TMA, TMB>(objectIds);
            _dispatchCallbacksById.Add(lookupId, callback);
            return objectIds;
        }

        private sealed class ListDoubleDispatchCallback<TMA, TMB> : IDoubleDispatchCallback
            where TMA : MDynamicEntity
            where TMB : MDynamicEntity
        {
            private readonly UniqueList<DoubleDispatchEntityIds<TMA, TMB>> _objectIds;

            public ListDoubleDispatchCallback(
                UniqueList<DoubleDispatchEntityIds<TMA, TMB>> objectIds
            )
            {
                _objectIds = objectIds;
            }

            public bool Dispatch(
                DoubleDispatchEntityIds<MDynamicEntity, MDynamicEntity> objectIds
            ) => _objectIds.Add(new(
                objectIds.LesserKindEntityId.Downcast<TMA>(),
                objectIds.GreaterKindEntityId.Downcast<TMB>()
            ));
        }
    }
}