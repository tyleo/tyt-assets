#nullable enable

using System;
using Tyleo.Game.Update;

namespace Tyleo.Game
{
    /// <summary>
    /// Represents the root object for a Tyleo scene. There should only be one
    /// such object in a scene.
    /// </summary>
    public interface IRootSceneObject :
        IDisposable,
        IRuntimeFixedUpdatable,
        IRuntimeUpdatable
    {
        /// <summary>
        /// Initializes the root scene object. The root scene object should
        /// register with
        /// <see cref="TyleoGameInstance.RetainRootSceneObject"/>
        /// in `Awake()` and return immediately. Any further initialization
        /// logic should be performed in this method.
        /// </summary>
        void Initialize();
    }
}