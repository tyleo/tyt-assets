#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for input actions.
    /// </summary>
    public interface IInputActionOps
    {
        /// <summary>
        /// Enables the input action.
        /// </summary>
        /// <param name="inputActionId">
        /// The id of the input action to enable.
        /// </param>
        void Enable(U32Id<MInputAction> inputActionId);

        /// <summary>
        /// Disables the input action.
        /// </summary>
        /// <param name="inputActionId">
        /// The id of the input action to disable.
        /// </param>
        void Disable(U32Id<MInputAction> inputActionId);

        /// <summary>
        /// Checks if the input action was pressed this frame.
        /// </summary>
        /// <param name="inputActionId">
        /// The id of the input action to check.
        /// </param>
        /// <returns>
        /// True if the input action was pressed this frame, false otherwise.
        /// </returns>
        bool WasPressedThisFrame(U32Id<MInputAction> inputActionId);

        /// <summary>
        /// Reads the value of the input action as a 2D vector.
        /// </summary>
        /// <param name="inputActionId">
        /// The id of the input action to read.
        /// </param>
        /// <returns>The value of the input action as a 2D vector.</returns>
        TyVector2 ReadVector2(U32Id<MInputAction> inputActionId);
    }
}