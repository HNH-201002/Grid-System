using UnityEngine;

namespace GridSystem.Utilities
{
    public class RaycastHandler
    {
        private Vector3 currentPosition = Vector3.positiveInfinity;
        private Vector3 hitPointPosition = Vector3.positiveInfinity;
        private const float epsilon = 0.01f;

        public Vector3? GetPositionFromRaycast(Camera camera, Vector3 mousePosition)
        {
            if (camera == null) return null;

            if (Vector3.Distance(currentPosition, mousePosition) < epsilon)
            {
                return hitPointPosition;
            }

            currentPosition = mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                hitPointPosition = hit.point;
                return hit.point;
            }

            return null;
        }
    }
}