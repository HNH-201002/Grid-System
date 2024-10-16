using UnityEngine;

namespace GridSystem.Visualization
{
    public class UIFollowCamera : MonoBehaviour
    {
        private Transform playerCamera;

        private void Start()
        {
            playerCamera = Camera.main.transform;
        }

        private void Update()
        {
            transform.LookAt(transform.position + playerCamera.rotation * Vector3.forward,
                             playerCamera.rotation * Vector3.up);
        }
    }

}