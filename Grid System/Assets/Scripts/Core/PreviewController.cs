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

        public PreviewController(BuildingPrefabData data, PreviewManager previewManager, Camera camera)
        {
            buildingPrefabData = data;
            this.previewManager = previewManager;
            mainCamera = camera;
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
            GridManager.Instance.SetGameState(GameState.Building);
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
            if (raycastHandler.RaycastNow(mainCamera, Input.mousePosition) is Vector3 hitPoint)
            {
                previewManager.ActivatePreview(new Vector3(hitPoint.x, 0.5f, hitPoint.z));
                previewManager.SetupPreviewComponent(this, buildingPrefabData);
                GridManager.Instance.TurnOffFootprint();
            }
        }

        public void SetDeactivePreviewMode()
        {
            currentState = PreviewState.Off;
        }
    }
}
