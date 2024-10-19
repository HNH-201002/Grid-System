using UnityEngine;
using System;

namespace GridSystem.InputManagement
{
    /// <summary>
    /// Handles user input for both mouse and touch interactions.
    /// Provides events for input clicks and holds, and methods to query input position and movement.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when the user performs a click or tap.
        /// </summary>
        public event Action OnInputClick;

        /// <summary>
        /// Event triggered while the user is holding a click or touch.
        /// </summary>
        public event Action OnInputHold;

        private void Update()
        {
            if (IsInputClicked())
            {
                OnInputClick?.Invoke();
            }

            if (IsInputHolding())
            {
                OnInputHold?.Invoke();
            }
        }

        /// <summary>
        /// Determines if the user has clicked or tapped.
        /// </summary>
        /// <returns>True if a click or tap is detected, otherwise false.</returns>
        public bool IsInputClicked()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            }

            return Input.GetMouseButtonDown(0);
        }

        /// <summary>
        /// Determines if the user is holding a click or touch.
        /// </summary>
        /// <returns>True if the input is being held, otherwise false.</returns>
        public bool IsInputHolding()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved;
            }

            return Input.GetMouseButton(0);
        }

        /// <summary>
        /// Checks if the input position has moved compared to a previous position.
        /// </summary>
        /// <param name="previousPosition">The previous input position to compare.</param>
        /// <returns>True if the input has moved, otherwise false.</returns>
        public bool HasInputMoved(Vector3 previousPosition)
        {
            return GetInputPosition() != previousPosition;
        }

        /// <summary>
        /// Gets the current position of the user's input.
        /// </summary>
        /// <returns>The position of the input as a <see cref="Vector3"/>.</returns>
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
