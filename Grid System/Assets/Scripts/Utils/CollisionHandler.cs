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
            if ((targetTag != null && collision.collider.CompareTag(targetTag)) ||
                (targetLayer != -1 && collision.gameObject.layer == targetLayer))
            {
                collisionCounter++;
                IsBuildable = false;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if ((targetTag != null && collision.collider.CompareTag(targetTag)) ||
                (targetLayer != -1 && collision.gameObject.layer == targetLayer))
            {
                collisionCounter--;
                IsBuildable = collisionCounter <= 0;
            }
        }
    }
}
