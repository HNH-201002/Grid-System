using UnityEngine;

namespace GridSystem.Utilities
{
    /// <summary>
    /// Handles raycasting operations with optional mouse movement tracking.
    /// Provides methods to perform conditional or immediate raycasts.
    /// </summary>
    public class RaycastHandler
    {
        private Vector3 lastMousePosition = Vector3.positiveInfinity;
        private Vector3 lastHitPoint = Vector3.positiveInfinity;
        private const float epsilon = 0.01f;

        /// <summary>
        /// Executes the raycast only if the mouse has moved significantly since the last check.
        /// </summary>
        public Vector3? RaycastIfMoved(Camera camera, Vector3 mousePosition, LayerMask? targetLayer = null)
        {
            if (camera == null) return null;

            if ((lastMousePosition - mousePosition).sqrMagnitude < epsilon * epsilon)
            {
                return lastHitPoint;
            }

            lastMousePosition = mousePosition;
            return DoRaycast(camera, mousePosition, targetLayer);
        }

        /// <summary>
        /// Performs the raycast immediately, without any condition checks.
        /// </summary>
        public Vector3? RaycastNow(Camera camera, Vector3 mousePosition, LayerMask? targetLayer = null)
        {
            if (camera == null) return null;

            return DoRaycast(camera, mousePosition, targetLayer);
        }

        private Vector3? DoRaycast(Camera camera, Vector3 mousePosition, LayerMask? targetLayer = null)
        {
            Ray ray = camera.ScreenPointToRay(mousePosition);

            int layerMaskValue = targetLayer?.value ?? ~0;

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layerMaskValue))
            {
                lastHitPoint = hit.point;
                return lastHitPoint;
            }

            return null;
        }
    }
}
