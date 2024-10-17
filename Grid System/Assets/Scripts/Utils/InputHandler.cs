using UnityEngine;
using System;

namespace GridSystem.InputManagement
{
    public class InputHandler : MonoBehaviour
    {
        public event Action OnInputClick;

        private Vector3 previousInputPosition;

        private void Update()
        {
            if (IsInputClicked())
            {
                OnInputClick?.Invoke();
            }

            previousInputPosition = GetInputPosition();
        }

        public bool IsInputClicked()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            }

            return Input.GetMouseButtonDown(0);
        }

        public bool IsInputHolding()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
            }

            return Input.GetMouseButton(0);
        }

        public bool HasInputMoved(Vector3 previousPosition)
        {
            return GetInputPosition() != previousPosition;
        }

        public Vector3 GetInputPosition()
        {
            if (Input.touchCount > 0)
            {
                return Input.GetTouch(0).position;
            }

            return Input.mousePosition;
        }
    }
}
