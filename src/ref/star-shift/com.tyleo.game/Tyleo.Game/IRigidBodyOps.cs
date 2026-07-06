#nullable enable

using Tyleo.IdSys;

namespace Tyleo.Game
{
    /// <summary>
    /// Operations for rigid bodies.
    /// </summary>
    public interface IRigidBodyOps
    {
        /// <summary>
        /// Gets the angular velocity of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The angular velocity of the rigid body.</returns>
        TyVector3 GetAngularVelocity(
            U32Id<MRigidBody> rigidBodyId
        );

        /// <summary>
        /// Gets the linear velocity of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The linear velocity of the rigid body.</returns>
        TyVector3 GetLinearVelocity(
            U32Id<MRigidBody> rigidBodyId
        );

        /// <summary>
        /// Gets the linear and angular velocity of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>
        /// The linear and angular velocity of the rigid body.
        /// </returns>
        (TyVector3 Linear, TyVector3 Angular) GetLinearAndAngularVelocity(
            U32Id<MRigidBody> rigidBodyId
        );

        /// <summary>
        /// Gets the world position of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The world position of the rigid body.</returns>
        TyVector3 GetWorldPosition(
            U32Id<MRigidBody> rigidBodyId
        );

        /// <summary>
        /// Moves the position of a rigid body to a new world position.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body to move.</param>
        /// <param name="position">
        /// The desired new position of the rigid body.
        /// </param>
        void MoveWorldPosition(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 position
        );

        /// <summary>
        /// Gets the world rotation of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The world rotation of the rigid body.</returns>
        TyQuaternion GetWorldRotation(
            U32Id<MRigidBody> rigidBodyId
        );

        /// <summary>
        /// Moves the rotation of a rigid body to a new world rotation.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body to move.</param>
        /// <param name="rotation">
        /// The desired new rotation of the rigid body.
        /// </param>
        void MoveWorldRotation(
            U32Id<MRigidBody> rigidBodyId,
            TyQuaternion rotation
        );

        /// <summary>
        /// Gets the world pose of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The world pose of the rigid body.</returns>
        TyPose GetWorldPose(
            U32Id<MRigidBody> rigidBodyId
        );

        /// <summary>
        /// Gets the lossy world TRS of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The lossy world TRS of the rigid body.</returns>
        TyTrs GetLossyWorldTrs(U32Id<MRigidBody> rigidBodyId);

        /// <summary>
        /// Gets the lossy world uniform scale of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <returns>The lossy world uniform scale of the rigid body.</returns>
        float GetLossyWorldUniformScale(U32Id<MRigidBody> rigidBodyId);

        /// <summary>
        /// Moves the position and rotation of a rigid body to new world
        /// position and rotation.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body to move.</param>
        /// <param name="position">
        /// The desired new position of the rigid body.
        /// </param>
        /// <param name="rotation">
        /// The desired new rotation of the rigid body.
        /// </param>
        void MoveWorldPositionAndRotation(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 position,
            TyQuaternion rotation
        );

        /// <summary>
        /// Sets the angular velocity of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="angularVelocity">The new angular velocity.</param>
        void SetAngularVelocity(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 angularVelocity
        );

        /// <summary>
        /// Sets whether a rigid body should detect collisions. If false, the
        /// rigid body will not detect collisions and will not be included in
        /// collision checks.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="detectCollisions">
        /// Whether the rigid body should detect collisions.
        /// </param>
        void SetDetectCollisions(
            U32Id<MRigidBody> rigidBodyId,
            bool detectCollisions
        );

        /// <summary>
        /// Sets the linear velocity of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="linearVelocity">The new linear velocity.</param>
        void SetLinearVelocity(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 linearVelocity
        );

        /// <summary>
        /// Sets the linear and angular velocity of a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="linearVelocity">The new linear velocity.</param>
        /// <param name="angularVelocity">The new angular velocity.</param>
        void SetLinearAndAngularVelocity(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 linearVelocity,
            TyVector3 angularVelocity
        );

        /// <summary>
        /// Teleports a rigid body to a new world position without
        /// interpolation.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="position">The new world position.</param>
        void SetDiscontinuousWorldPosition(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 position
        );

        /// <summary>
        /// Teleports a rigid body to a new world rotation without
        /// interpolation.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="rotation">The new world rotation.</param>
        void SetDiscontinuousWorldRotation(
            U32Id<MRigidBody> rigidBodyId,
            TyQuaternion rotation
        );

        /// <summary>
        /// Teleports a rigid body to a new world position and rotation
        /// without interpolation.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="position">The new world position.</param>
        /// <param name="rotation">The new world rotation.</param>
        void SetDiscontinuousWorldPositionAndRotation(
            U32Id<MRigidBody> rigidBodyId,
            TyVector3 position,
            TyQuaternion rotation
        );

        /// <summary>
        /// Sets whether a rigid body is kinematic.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        /// <param name="isKinematic">
        /// Whether the rigid body should be kinematic.
        /// </param>
        void SetIsKinematic(
            U32Id<MRigidBody> rigidBodyId,
            bool isKinematic
        );

        /// <summary>
        /// Puts a rigid body to sleep.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        void Sleep(U32Id<MRigidBody> rigidBodyId);

        /// <summary>
        /// Zeros out linear and angular velocity and puts the rigid body to
        /// sleep.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        void StopAndSleep(U32Id<MRigidBody> rigidBodyId);

        /// <summary>
        /// Wakes up a rigid body.
        /// </summary>
        /// <param name="rigidBodyId">The id of the rigid body.</param>
        void WakeUp(U32Id<MRigidBody> rigidBodyId);
    }
}