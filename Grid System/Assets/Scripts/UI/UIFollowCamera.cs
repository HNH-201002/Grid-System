using UnityEngine;

namespace GridSystem.Visualization
{
    /// <summary>
    /// Makes a UI element or object always face the player's camera.
    /// Useful for 3D UI elements that need to stay oriented towards the player.
    /// </summary>
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