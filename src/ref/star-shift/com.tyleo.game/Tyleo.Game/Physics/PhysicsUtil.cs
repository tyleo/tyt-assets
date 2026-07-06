#nullable enable

using Tyleo.Collections;
using Tyleo.Extensions;

namespace Tyleo.Game.Physics
{
    /// <summary>
    /// Utilities for physics.
    /// </summary>
    public static class PhysicsUtil
    {
        /// <summary>
        /// Encapsulates all bounds in the given enumerable.
        /// </summary>
        /// <typeparam name="TBoundsEnumerator">
        /// The enumerator type for the bounds.
        /// </typeparam>
        /// <param name="boundsEnumerable">The bounds to encapsulate.</param>
        /// <returns>The encapsulated bounds.</returns>
        public static TyBounds EncapsulateAllBounds<TBoundsEnumerator>(
            ValueEnumerable<TyBounds, TBoundsEnumerator> boundsEnumerable
        ) where TBoundsEnumerator : struct, IValueEnumerator<TyBounds>
        {
            return boundsEnumerable.Aggregate(
                TyBounds.Zero,
                (current, next) => current.Encapsulate(next)
            );
        }

        /// <summary>
        /// Calculates the per-collider poses in root space for a single
        /// set of colliders, accounting for a scaled root transform.
        /// </summary>
        public static TyPose[] CalculateScaledColliderPosesInRoot<TUniformTrsEnumerator>(
            TyUniformTrs rootWorldTrs,
            ValueEnumerable<TyUniformTrs, TUniformTrsEnumerator> boundsWorldTrsEnumerable
        )
            where TUniformTrsEnumerator : struct, IValueEnumerator<TyUniformTrs>
        {
            return rootWorldTrs
                .SelectRelativeTrs(boundsWorldTrsEnumerable)
                .Select(i => i.ToPose())
                .ToArray();
        }

        /// <summary>
        /// Calculates the per-collider poses in root space for a single
        /// set of colliders, ignoring scale.
        /// </summary>
        public static TyPose[] CalculateUnscaledColliderPosesInRoot<TPoseEnumerator>(
            TyPose rootWorldPose,
            ValueEnumerable<TyPose, TPoseEnumerator> boundsWorldPoseEnumerable
        )
            where TPoseEnumerator : struct, IValueEnumerator<TyPose>
        {
            return rootWorldPose
                .SelectRelativePose(boundsWorldPoseEnumerable)
                .ToArray();
        }

        /// <summary>
        /// Encapsulates the world-space collider bounds into a single
        /// combined bounds in root space, given the precomputed
        /// collider-to-root transforms.
        /// </summary>
        public static TyBounds EncapsulateScaledColliderBoundsInRoot<TTransformInRootEnumerator, TBoundsEnumerator>(
            ValueEnumerable<TyUniformTrs, TTransformInRootEnumerator> transformsInRoot,
            ValueEnumerable<TyBounds, TBoundsEnumerator> worldBoundsEnumerable
        )
            where TTransformInRootEnumerator : struct, IValueEnumerator<TyUniformTrs>
            where TBoundsEnumerator : struct, IValueEnumerator<TyBounds>
        {
            var boundsInRoot = worldBoundsEnumerable
                .Zip(transformsInRoot)
                .Select(i => i.Item2.TransformAabbConservative(i.Item1));
            return EncapsulateAllBounds(boundsInRoot);
        }

        /// <summary>
        /// Encapsulates the world-space collider bounds into a single
        /// combined bounds in root space, given the precomputed
        /// collider-to-root poses.
        /// </summary>
        public static TyBounds EncapsulateUnscaledColliderBoundsInRoot<TPoseInRootEnumerator, TBoundsEnumerator>(
            ValueEnumerable<TyPose, TPoseInRootEnumerator> posesInRoot,
            ValueEnumerable<TyBounds, TBoundsEnumerator> worldBoundsEnumerable
        )
            where TPoseInRootEnumerator : struct, IValueEnumerator<TyPose>
            where TBoundsEnumerator : struct, IValueEnumerator<TyBounds>
        {
            var boundsInRoot = worldBoundsEnumerable
                .Zip(posesInRoot)
                .Select(i => i.Item2.TransformAabbConservative(i.Item1));
            return EncapsulateAllBounds(boundsInRoot);
        }
    }
}
