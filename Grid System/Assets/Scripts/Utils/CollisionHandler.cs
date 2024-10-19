using UnityEngine;

namespace GridSystem.Core.PhysicsCollider
{
    /// <summary>
    /// Handles collision detection and determines if a building placement is valid.
    /// Manages collision counts to track if the placement area is obstructed by other objects.
    /// </summary>
    public class CollisionHandler : MonoBehaviour
    {
        /// <summary>
        /// Gets a value indicating whether the current area is buildable.
        /// </summary>
        public bool IsBuildable { get; private set; } = true;

        private int collisionCounter = 0;
        private string targetTag = null;
        private int targetLayer = -1;

        public void Initialize(string tag = null, int layer = -1)
        {
            targetTag = tag;
            targetLayer = layer;
        }

        private void OnEnable()
        {
            collisionCounter = 0;
            IsBuildable = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsRelevantCollision(collision.collider))
            {
                collisionCounter++;
                IsBuildable = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsRelevantCollision(collision.collider))
            {
                collisionCounter--;
                IsBuildable = collisionCounter <= 0;
            }
        }

        private bool IsRelevantCollision(Collider collider)
        {
            return (targetTag != null && collider.CompareTag(targetTag)) ||
                (targetLayer != -1 && gameObject.layer == targetLayer);
        }
    }
}
