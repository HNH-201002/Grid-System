using UnityEngine;
using GridSystem.Core;

namespace GridSystem.UIManager
{
    /// <summary>
    /// Manages the currently active building preview selector.
    /// Ensures only one preview manager is active at a time.
    /// </summary>
    public class BuildingSelectorManager : MonoBehaviour
    {
        private PreviewManager CurrentSelector;

        /// <summary>
        /// Sets a new active preview selector and hides the previous one if it exists.
        /// </summary>
        /// <param name="newSelector">The new <see cref="PreviewManager"/> to set as active.</param>
        public void SetActiveSelector(PreviewManager newSelector)
        {
            if (CurrentSelector != null && CurrentSelector != newSelector)
            {
                CurrentSelector.HidePreview();
            }

            CurrentSelector = newSelector;
        }
    }
}
