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
                GridManager.Instance.SetGameState(GameState.None);
                CurrentSelector.HidePreview();
            }

            CurrentSelector = newSelector;
        }
    }
}
