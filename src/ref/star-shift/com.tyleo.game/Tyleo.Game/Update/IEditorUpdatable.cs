#nullable enable

namespace Tyleo.Game.Update
{
    /// <summary>
    /// Represents an object that can only be updated in the editor.
    /// </summary>
    public interface IEditorUpdatable
    {
        /// <summary>
        /// Performs an editor update.
        /// </summary>
        /// <param name="args">
        /// Arguments providing context for the update.
        /// </param>
        void EditorUpdate(in EditorUpdateArgs args);
    }
}