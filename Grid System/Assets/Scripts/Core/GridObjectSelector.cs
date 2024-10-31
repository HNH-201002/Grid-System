using UnityEngine;
using System;
using GridSystem.InputManagement;
using GridSystem.Utilities;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    /// <summary>
    /// Handles the selection of buildings within the grid system.
    /// Manages user input, object selection, and displays footprints for selected buildings.
    /// </summary>
    public class GridObjectSelector : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when a building object is selected.
        /// </summary>
        public event Action<Building> ObjectSelect;

        private IBuildingManager buildingManager;
        private IGridBehavior gridBehavior;
        private InputHandler inputHandler;
        private RaycastHandler raycastHandler;
        private Camera mainCamera;
        private GameObject footprintGO;
        private Building currentBuiltObject;
        private FootprintGenerator footprintGenerator;
        private GridManager gridManager;

        public void Initialize(GridManager gridManager, IBuildingManager buildingManager, IGridBehavior gridBehavior, InputHandler inputHandler, Camera mainCamera)
        {
            this.gridManager = gridManager;
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
            if (gridManager.CurrentGameState == GridState.Building)
                return;

            Vector3? position = raycastHandler.RaycastNow(mainCamera, inputHandler.GetInputPosition());
            if (!position.HasValue)
                return;

            Building selectedObject = gridBehavior.GetBuildingAtGrid(position.Value);

            UpdateSelectedObject(selectedObject);
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

        /// <summary>
        /// Turns off the footprint display and resets the current selection.
        /// </summary>
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
