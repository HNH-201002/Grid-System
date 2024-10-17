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
            if (GridManager.Instance.CurrentGameState == GameState.Building)
                return;

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return;
            }
            else if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3? position = raycastHandler.RaycastNow(mainCamera, inputHandler.GetInputPosition());
            if (!position.HasValue) return;
            Building builtObject = gridBehavior.GetBuiltObjectInGrid(position.Value);
            if (builtObject == null) return;

            if (currentBuiltObject != builtObject)
            {
                currentBuiltObject?.ToggleFootprintDisplay(footprintGO.transform);
                builtObject.ToggleFootprintDisplay(footprintGO.transform);
                currentBuiltObject = builtObject;
                ObjectSelect?.Invoke(currentBuiltObject);
            }
            else
            {
                currentBuiltObject.ToggleFootprintDisplay(footprintGO.transform);
            }
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
