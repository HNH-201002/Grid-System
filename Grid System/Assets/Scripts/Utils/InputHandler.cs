using UnityEngine;
using System;

namespace GridSystem.InputManagement
{
    public class InputHandler : MonoBehaviour
    {
        public event Action OnMouseClick;

        private void Update()
        {
            if (IsMouseClicked())
            {
                OnMouseClick?.Invoke();
            }
        }

        public bool IsMouseClicked()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool IsMouseHolding()
        {
            return Input.GetMouseButton(0);
        }

        public bool HasMouseMoved(Vector3 previousMousePosition)
        {
            return Input.mousePosition != previousMousePosition;
        }

        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }
    }
}
