using UnityEngine;
using GridSystem.Core;

namespace GridSystem.UIManager
{
    public class BuildingSelectorManager : MonoBehaviour
    {
        private PreviewManager CurrentSelector;

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
