using UnityEngine;

namespace GridSystem.Core.PhysicsCollider
{
    public class CollisionHandler : MonoBehaviour
    {
        private int collisionCounter = 0;
        public bool IsBuildable { get; private set; } = true;

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
