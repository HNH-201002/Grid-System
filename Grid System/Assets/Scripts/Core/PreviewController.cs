using UnityEngine;
using GridSystem.Data;
using GridSystem.Core;

namespace GridSystem.UIManager
{
    public class PreviewController
    {
        private BuildingPrefabData buildingPrefabData;
        private PreviewManager previewManager;

        public enum PreviewState { Off, Active }
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

        public void SetDeactivePreviewMode()
        {
            currentState = PreviewState.Off;
        }
    }
}
