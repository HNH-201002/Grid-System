using UnityEngine;
using GridSystem.Data;
using GridSystem.Core;

namespace GridSystem.UIManager
{
    /// <summary>
    /// Controls the preview mode for building placement within the grid system.
    /// </summary>
    public class PreviewController
    {
        /// <summary>
        /// Enum representing the state of the preview.
        /// </summary>
        public enum PreviewState { Off, Active }

        private BuildingPrefabData buildingPrefabData;
        private PreviewManager previewManager;

        private PreviewState currentState = PreviewState.Off;

        private Vector3 initialSpawnPoint;

        private GridManager gridManager;

        public PreviewController(GridManager gridManager, BuildingPrefabData data, PreviewManager previewManager, Vector3 initialSpawnPoint)
        {
            this.gridManager = gridManager;
            buildingPrefabData = data;
            this.previewManager = previewManager;
            this.initialSpawnPoint = initialSpawnPoint;
        }

        /// <summary>
        /// Toggles the state of the preview between Active and Off.
        /// </summary>
        public void TogglePreviewState()
        {
            if (currentState == PreviewState.Off)
            {
                ActivatePreviewMode();
            }
            else
            {
                DeactivatePreviewMode();
            }
        }

        private void ActivatePreviewMode()
        {
            currentState = PreviewState.Active;
            gridManager.SetActiveSelector(previewManager);
            ActivatePreview();
        }

        private void DeactivatePreviewMode()
        {
            gridManager.SetActiveSelector(null);
            previewManager.HidePreview();
            currentState = PreviewState.Off;
        }

        private void ActivatePreview()
        {
            previewManager.ActivatePreview(initialSpawnPoint);
            previewManager.SetupPreviewComponent(this, buildingPrefabData);
            gridManager.TurnOffFootprint();
        }

        /// <summary>
        /// Sets the state of the preview to Off.
        /// </summary>
        public void SetDeactivePreviewMode()
        {
            currentState = PreviewState.Off;
        }
    }
}
