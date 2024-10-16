using GridSystem.Data;
using GridSystem.Visualization;
using UnityEngine;
using UnityEngine.UI;
using GridSystem.Core;
using GridSystem.Pooling;
using GridSystem.Utilities;
using System;

namespace GridSystem.UIManager
{
    public class BuildingTypeSelector : MonoBehaviour
    {
        [SerializeField]
        private ObjectInteractionUI objectInteractionUI;

        [SerializeField]
        private GridObjectSelector gridObjectSelector;

        [SerializeField]
        private GridBehavior gridBehavior;

        [SerializeField]
        private BuildingPrefabData buildingPrefabData;

        [SerializeField]
        private Button button;

        [SerializeField]
        private BuildingTypeManager buildingTypeManager;

        private bool isToggled;

        private ObjectPool<GameObject> previewPool;

        private GameObject previewInstance;

        private PlacementPreview previewComponent;

        private Camera mainCamera;

        private RaycastHandler raycastHandler = new RaycastHandler();

        private void Start()
        {
            mainCamera = Camera.main;

            if (buildingPrefabData.Prefab == null)
            {
                Debug.LogError("Prefab is missing. Please assign the prefab in BuildingPrefabData.");
                button.interactable = false;
                return;
            }

            previewPool = new ObjectPool<GameObject>(buildingPrefabData.Prefab, 1);
            button.onClick.AddListener(TogglePreview);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(TogglePreview);
        }

        private void TogglePreview()
        {
            SetToggleState(!isToggled);

            if (isToggled)
            {
                GameStateManager.SetGameState(GameState.Building);
            }
            else
            {
                GameStateManager.SetGameState(GameState.None);
            }

            if (isToggled)
            {
                ActivatePreview();
            }
            else
            {
                HidePreviewInstance();
            }
        }

        private void SetToggleState(bool toggled)
        {
            isToggled = toggled;
            buildingTypeManager.SetActiveSelector(isToggled ? this : null);
        }

        private void ActivatePreview()
        {
            if (previewInstance == null)
            {
                previewInstance = previewPool.GetFromPool();
                SetupPreviewComponent();
            }

            if (GetMouseWorldPosition(out Vector3 hitPoint))
            {
                previewInstance.transform.position = new Vector3(hitPoint.x, 0.5f, hitPoint.z);
            }

            gridObjectSelector.TurnOffFootprint();
        }

        private bool GetMouseWorldPosition(out Vector3 hitPoint)
        {
            Vector3? position = raycastHandler.GetPositionFromRaycast(mainCamera, Input.mousePosition);

            if (position != null)
            {
                hitPoint = position.Value;
                return true;
            }

            hitPoint = Vector3.zero;
            return false;
        }

        private void SetupPreviewComponent()
        {
            previewComponent = previewInstance.GetComponent<PlacementPreview>() ?? previewInstance.AddComponent<PlacementPreview>();
            previewComponent.Initialize(gridBehavior, this, buildingPrefabData.BuildingType);
            previewComponent.ResetPreview();
            objectInteractionUI.SetPlacementPreviewInstanec(previewComponent);
        }

        public void HidePreviewInstance()
        {
            if (previewInstance != null)
            {
                previewPool.ReturnToPool(previewInstance);
                previewInstance = null;
                previewComponent = null;
                isToggled = false;
                GameStateManager.SetGameState(GameState.None);
            }
        }
    }
}
