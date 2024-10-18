using UnityEngine;
using UnityEngine.EventSystems;
using System;
using GridSystem.InputManagement;
using GridSystem.Utilities;

namespace GridSystem.Core
{
    public class GridObjectSelector : MonoBehaviour
    {
        private BuildingManager buildingManager;

        private GridBehavior gridBehavior;

        private InputHandler inputHandler;

        private RaycastHandler raycastHandler;
        private Camera mainCamera;
        private GameObject footprintGO;
        private Building currentBuiltObject;
        private FootprintGenerator footprintGenerator;

        public event Action<Building> ObjectSelect;

        public void Initialize(BuildingManager buildingManager, GridBehavior gridBehavior, InputHandler inputHandler, Camera mainCamera)
        {
            this.buildingManager = buildingManager;
            this.gridBehavior = gridBehavior;
            this.inputHandler = inputHandler;
            this.mainCamera = mainCamera;
        }

        private void Start()
        {
            raycastHandler = new RaycastHandler();
            footprintGenerator = new FootprintGenerator();
            footprintGO = footprintGenerator.Generate();

            inputHandler.OnInputClick += HandleGridObjectSelection;
            buildingManager.BuildingRemoved += OnBuildingRemoved;
        }

        private void OnDestroy()
        {
            inputHandler.OnInputClick -= HandleGridObjectSelection;
            buildingManager.BuildingRemoved -= OnBuildingRemoved;
        }

        private void HandleGridObjectSelection()
        {
            if (GridManager.Instance.CurrentGameState == GameState.Building) return;

            if (IsPointerOverUI()) return;

            Vector3? position = raycastHandler.RaycastNow(mainCamera, inputHandler.GetInputPosition());
            if (!position.HasValue) return;

            Building selectedObject = gridBehavior.GetBuiltObjectInGrid(position.Value);

            UpdateSelectedObject(selectedObject);
        }

        private bool IsPointerOverUI()
        {
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);

            return EventSystem.current.IsPointerOverGameObject();
        }

        private void UpdateSelectedObject(Building selectedObject)
        {
            if (currentBuiltObject == selectedObject)
            {
                currentBuiltObject?.ToggleFootprintDisplay(footprintGO.transform);
                return;
            }

            currentBuiltObject?.ToggleFootprintDisplay(footprintGO.transform);

            currentBuiltObject = selectedObject;
            currentBuiltObject?.ToggleFootprintDisplay(footprintGO.transform);

            ObjectSelect?.Invoke(currentBuiltObject);
        }

        private void OnBuildingRemoved(Building build)
        {
            build.ToggleFootprintDisplay(footprintGO.transform);
            currentBuiltObject = null;
        }

        public void TurnOffFootprint()
        {
            if (currentBuiltObject != null)
            {
                currentBuiltObject.Reset();
                currentBuiltObject = null;
            }

            footprintGO.SetActive(false);
        }
    }
}
