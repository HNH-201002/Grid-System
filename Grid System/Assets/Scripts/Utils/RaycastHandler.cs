using UnityEngine;

namespace GridSystem.Utilities
{
    public class RaycastHandler
    {
        private Vector3 lastMousePosition = Vector3.positiveInfinity;
        private Vector3 lastHitPoint = Vector3.positiveInfinity;
        private const float epsilon = 0.01f;

        /// <summary>
        /// Executes the raycast only if the mouse has moved significantly since the last check.
        /// </summary>
        public Vector3? RaycastIfMoved(Camera camera, Vector3 mousePosition)
        {
            if (camera == null) return null;

            if (Vector3.Distance(lastMousePosition, mousePosition) < epsilon)
            {
                return lastHitPoint;
            }

            lastMousePosition = mousePosition;
            return DoRaycast(camera, mousePosition);
        }

        /// <summary>
        /// Performs the raycast immediately, without any condition checks.
        /// </summary>
        public Vector3? RaycastNow(Camera camera, Vector3 mousePosition)
        {
            if (camera == null) return null;

            return DoRaycast(camera, mousePosition);
        }

        private Vector3? DoRaycast(Camera camera, Vector3 mousePosition)
        {
            Ray ray = camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                lastHitPoint = hit.point;
                return hit.point;
            }

            return Vector3.zero;
        }
    }
}
