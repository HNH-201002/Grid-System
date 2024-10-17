using UnityEngine;
using GridSystem.InputManagement;
using System.Collections.Generic;
using GridSystem.Data;
using System;
using GridSystem.UIManager;
using GridSystem.Visualization;

namespace GridSystem.Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [Tooltip("How many cells within 1 unit")]
        [SerializeField]
        private int cellPerUnit = 1;

        [SerializeField]
        private Material buildableMaterial;

        public Material BuildableMaterial => buildableMaterial;

        [SerializeField]
        private Material unbuildableMaterial;

        public Material UnBuildableMaterial => unbuildableMaterial;

        [SerializeField]
        private List<BuildingPrefabData> buildingDataList;

        [SerializeField]
        private Camera mainCamera;

        public static GridManager Instance { get; private set; }

        public float GridSizeX { get; private set; }
        public float GridSizeZ { get; private set; }

        public int GridWidth { get; private set; }

        public Vector3 MinScaled { get; private set; }
        public Vector3 MaxScaled { get; private set; }

        private BuildingManager buildingManager;

        private GridBehavior gridBehavior;

        private InputHandler inputHandler;

        private GridObjectSelector gridObjectSelector;

        private BuildingSelectorManager buildingTypeManager;

        private BuildingManipulator buildingManipulator;

        public event Action<Building> ObjectSelected;

        public event Action BuildCompleted;

        public GameState CurrentGameState { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                GridCalculator calculator = gameObject.AddComponent<GridCalculator>();
                calculator.Initialize(meshFilter, cellPerUnit);
                var (gridSizeX, gridSizeZ, minScaled, maxScaled, gridWidth) = calculator.CalculateGridSize(transform.lossyScale);

                GridSizeX = gridSizeX;
                GridSizeZ = gridSizeZ;
                MinScaled = minScaled;
                MaxScaled = maxScaled;
                GridWidth = gridWidth;
            }
            else
            {
                Destroy(gameObject);
            }

            if (!TryGetComponent(out buildingManager))
            {
                buildingManager = gameObject.AddComponent<BuildingManager>();
            }

            if (!TryGetComponent(out gridBehavior))
            {
                gridBehavior = gameObject.AddComponent<GridBehavior>();
            }

            if (!TryGetComponent(out inputHandler))
            {
                inputHandler = gameObject.AddComponent<InputHandler>();
            }

            if (!TryGetComponent(out gridObjectSelector))
            {
                gridObjectSelector = gameObject.AddComponent<GridObjectSelector>();
            }

            if (!TryGetComponent(out buildingTypeManager))
            {
                buildingTypeManager = gameObject.AddComponent<BuildingSelectorManager>();
            }


            if (buildingManipulator == null)
            {
                buildingManipulator = new BuildingManipulator();
            }

            SetUp();
            RegisterEvents();
            SetGameState(GameState.None);
        }

        private void SetUp()
        {
            gridBehavior.Initialize(buildingManager);
            gridObjectSelector.Initialize(buildingManager, gridBehavior, inputHandler, mainCamera);
            buildingManager.Initialize(buildingDataList, gridBehavior);
        }

        private void RegisterEvents()
        {
            if (gridObjectSelector != null)
            {
                gridObjectSelector.ObjectSelect += OnObjectSelected;
                buildingManipulator.BuildingCompleted += OnBuildingCompleted;
            }
        }

        private void OnObjectSelected(Building building)
        {
            ObjectSelected?.Invoke(building);
        }

        private void OnBuildingCompleted()
        {
            BuildCompleted?.Invoke();
        }

        public void TurnOffFootprint()
        {
            gridObjectSelector.TurnOffFootprint();
        }

        public int GetGridIndexFromPosition(Vector3 position)
        {
            return gridBehavior.GetGridIndexFromPosition(position);
        }

        public void PlacePrefabOnGrid(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider)
        {
            gridBehavior.PlacePrefabOnGrid(position, rotation, buildingType, boxCollider);
        }

        public Vector3 GetGridPosition(int gridIndex)
        {
            return gridBehavior.GetGridPosition(gridIndex);
        }

        public void RemoveBuilding(Building building)
        {
            buildingManager.Remove(building);
        }

        public void SetGameState(GameState newGameState)
        {
            CurrentGameState = newGameState;
        }

        public void SetActiveSelector(PreviewManager newSelector)
        {
            buildingTypeManager.SetActiveSelector(newSelector);
        }

        public void DeleteBuilding()
        {
            buildingManipulator.DeleteBuilding();
        }

        public void RotateLeftBuilding()
        {
            buildingManipulator.RotateLeft();
        }

        public void RotateRightBuilding()
        {
            buildingManipulator.RotateRight();
        }

        public void AcceptPlacement()
        {
            buildingManipulator.AcceptPlacement();
        }

        public void SetBuildingInstance(Building building)
        {
            buildingManipulator.SetBuilding(building);
        }

        public void SetPlacementPreviewInstance(PlacementPreview placementPreview)
        {
            buildingManipulator.SetPlacementPreview(placementPreview);
        }

        private void OnDestroy()
        {
            gridObjectSelector.ObjectSelect -= OnObjectSelected;
            buildingManipulator.BuildingCompleted -= OnBuildingCompleted;
        }
    }
}