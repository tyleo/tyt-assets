#nullable enable

using System;
using System.Threading.Tasks;
using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Dependencies required by Tyleo games.
    /// </summary>
    public interface IGameDependencies
    {
        /// <summary>
        /// Draws a debug line from <paramref name="from"/> to
        /// <paramref name="to"/>.
        /// </summary>
        /// <param name="from">The start position.</param>
        /// <param name="to">The end position.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="duration">
        /// How long the line should be visible, in seconds. A duration of 0
        /// means the line is visible for one frame.
        /// </param>
        void DrawLine(
            TyVector3 from,
            TyVector3 to,
            TyRgbaColor color,
            float duration
        );

        /// <summary>
        /// Loads an engine scene asynchronously.
        /// </summary>
        /// <param name="sceneId">The id of the scene to load.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LoadEngineSceneAsync(U32Id<MScene> sceneId);

        /// <summary>
        /// Materializes the root behaviour for the runtime.
        /// </summary>
        /// <returns>The new root behaviour.</returns>
        object MaterializeRootBehaviour();

        /// <summary>
        /// Gets the last change to the configuration in UTC.
        /// </summary>
        /// <returns>
        /// The last configuration change time in UTC, or null if not available.
        /// </returns>
        DateTime? GetLastConfigChangeTimeUtc();

        /// <summary>
        /// Reloads the configuration for the game.
        /// </summary>
        void ReloadConfig();
    }
}