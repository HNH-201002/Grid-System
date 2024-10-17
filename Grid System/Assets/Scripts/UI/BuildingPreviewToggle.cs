using GridSystem.Data;
using UnityEngine;
using UnityEngine.UI;
using GridSystem.Core;
using GridSystem.Utilities;

namespace GridSystem.UIManager
{
    public class BuildingPreviewToggle : MonoBehaviour
    {
        [SerializeField]
        private BuildingPrefabData buildingPrefabData;

        [SerializeField]
        private Button button;

        private Camera mainCamera;
        private PreviewManager previewManager;
        private RaycastHandler raycastHandler = new RaycastHandler();
        private enum PreviewState { Off, Active }
        private PreviewState currentState = PreviewState.Off;

        private void Start()
        {
            mainCamera = Camera.main;

            if (buildingPrefabData.Prefab == null)
            {
                Debug.LogError("Prefab is missing. Please assign the prefab in BuildingPrefabData.");
                button.interactable = false;
                return;
            }

            previewManager = new PreviewManager(buildingPrefabData.Prefab);
            button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(HandleButtonClick);
        }

        private void HandleButtonClick()
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
