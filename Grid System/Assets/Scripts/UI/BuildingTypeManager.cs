using UnityEngine;
using GridSystem.Core;

namespace GridSystem.UIManager
{
    public class BuildingTypeManager : MonoBehaviour
    {
        public BuildingTypeSelector CurrentSelector { get; private set; }

        public void SetActiveSelector(BuildingTypeSelector newSelector)
        {
            if (CurrentSelector != null && CurrentSelector != newSelector)
            {
                GameStateManager.SetGameState(GameState.None);
                CurrentSelector.HidePreviewInstance();
            }

            CurrentSelector = newSelector;
        }
    }
}
