using UnityEngine;
using GridSystem.Data;
using GridSystem.Core;
using GridSystem.Utilities;

namespace GridSystem.UIManager
{
    public class PreviewController
    {
        private BuildingPrefabData buildingPrefabData;
        private Camera mainCamera;
        private PreviewManager previewManager;
        private RaycastHandler raycastHandler = new RaycastHandler();

        public enum PreviewState { Off, Active }
        private PreviewState currentState = PreviewState.Off;

        private Vector3 initialSpawnPoint;

        public PreviewController(BuildingPrefabData data, PreviewManager previewManager, Camera camera, Vector3 initialSpawnPoint)
        {
            buildingPrefabData = data;
            this.previewManager = previewManager;
            mainCamera = camera;
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
            GridManager.Instance.SetActiveSelector(previewManager);
            ActivatePreview();
        }

        private void DeactivatePreviewMode()
        {
            GridManager.Instance.SetActiveSelector(null);
            previewManager.HidePreview();
            currentState = PreviewState.Off;
        }

        private void ActivatePreview()
        {
            previewManager.ActivatePreview(initialSpawnPoint);
            previewManager.SetupPreviewComponent(this, buildingPrefabData);
            GridManager.Instance.TurnOffFootprint();
        }

        public void SetDeactivePreviewMode()
        {
            currentState = PreviewState.Off;
        }
    }
}
