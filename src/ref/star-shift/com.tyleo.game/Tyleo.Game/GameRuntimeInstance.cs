#nullable enable

using System;
using System.Threading.Tasks;
using Tyleo.IdSys;
using Tyleo.Game.Update;

namespace Tyleo.Game
{
    public sealed class GameRuntimeInstance : IDisposable
    {
        private readonly object _rootBehaviour;
        private IRootSceneObject? _rootSceneObject = default;
        private Task? _activeSceneLoadTask = default;

        public bool IsSceneLoading => _activeSceneLoadTask != null;

        private GameRuntimeInstance(
            object rootBehaviour
        )
        {
            _rootBehaviour = rootBehaviour;
        }

        internal static GameRuntimeInstance New(
            object rootBehaviour
        ) => new(
            rootBehaviour: rootBehaviour
        );

        /// <summary>
        /// Called when the module is uninitialized at runtime. This happens
        /// when exiting play mode in the Unity Editor. It's used to clean up
        /// resources that should not persist after play mode ends.
        /// </summary>
        public void Dispose()
        {
            _rootSceneObject?.Dispose();
        }

        /// <summary>
        /// Retains the root scene object for the current scene.
        /// </summary>
        /// <param name="rootSceneObject">
        /// The root scene object to register.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a root scene object is already registered.
        /// </exception>
        internal void RetainRootSceneObject(
            IRootSceneObject rootSceneObject
        )
        {
            if (_rootSceneObject != null) throw new InvalidOperationException(
                "A root scene object is already registered."
            );

            _rootSceneObject = rootSceneObject;
            rootSceneObject.Initialize();
        }

        /// <summary>
        /// Releases the root scene object for the current scene.
        /// </summary>
        internal void ReleaseRootSceneObject()
        {
            _rootSceneObject?.Dispose();
            _rootSceneObject = default;
        }

        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="sceneId">The Id of the scene to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if a scene is already being loaded.
        /// </exception>
        public async Task LoadSceneAsync(U32Id<MScene> sceneId)
        {
            if (_activeSceneLoadTask != null) throw new InvalidOperationException(
                "A scene is already being loaded."
            );
            _activeSceneLoadTask = LoadSceneInternalAsync(sceneId);
            await _activeSceneLoadTask;
            _activeSceneLoadTask = null;
        }

        private async Task LoadSceneInternalAsync(U32Id<MScene> sceneId)
        {
            ReleaseRootSceneObject();
            var dependencies = GameModule.Instance.Dependencies;
            await dependencies.LoadEngineSceneAsync(sceneId);
        }

        /// <summary>
        /// Performs an update.
        /// </summary>
        /// <param name="args">
        /// Arguments providing context for the runtime update.
        /// </param>
        internal void Update(in RuntimeUpdateArgs args) =>
            _rootSceneObject?.RuntimeUpdate(args);

        /// <summary>
        /// Performs a fixed update.
        /// </summary>
        /// <param name="args">
        /// Arguments providing context for the fixed update.
        /// </param>
        internal void FixedUpdate(in FixedUpdateArgs args) =>
            _rootSceneObject?.RuntimeFixedUpdate(args);
    }
}