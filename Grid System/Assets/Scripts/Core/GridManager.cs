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

        [Tooltip("Initial spawn position of the building")]
        [SerializeField]
        private readonly Vector3 initialSpawnPoint = new Vector3(0, 0.5f, 0);

        public static GridManager Instance { get; private set; }

        public float GridSizeX { get; private set; }
        public float GridSizeZ { get; private set; }

        public int GridWidth { get; private set; }

        public Vector3 MinScaled { get; private set; }
        public Vector3 MaxScaled { get; private set; }

        private IBuildingManager buildingManager;

        private IGridBehavior gridBehavior;

        private InputHandler inputHandler;

        private GridObjectSelector gridObjectSelector;

        private BuildingSelectorManager buildingTypeManager;

        private BuildingManipulator buildingManipulator;

        public event Action<Building> ObjectSelected;

        public event Action BuildCompleted;

        public GameState CurrentGameState { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeGrid();
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeComponents();
            SetUp();
            RegisterEvents();
            SetGameState(GameState.None);
        }

        private void InitializeComponents()
        {
            buildingManager = GetOrAddComponent<BuildingManager>();
            gridBehavior = GetOrAddComponent<GridBehavior>();
            inputHandler = GetOrAddComponent<InputHandler>();
            gridObjectSelector = GetOrAddComponent<GridObjectSelector>();
            buildingTypeManager = GetOrAddComponent<BuildingSelectorManager>();

            buildingManipulator ??= new BuildingManipulator(this);
        }

        private void SetUp()
        {
            gridBehavior.Initialize(this, buildingManager);
            gridObjectSelector.Initialize(this, buildingManager, gridBehavior, inputHandler, mainCamera);
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
            return gridBehavior.GetGridIndex(position);
        }

        public void PlacePrefabOnGrid(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider)
        {
            gridBehavior.PlaceBuilding(position, rotation, buildingType, boxCollider);
        }

        public Vector3 GetGridPosition(int gridIndex)
        {
            return gridBehavior.GetGridWorldPosition(gridIndex);
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

        public PreviewController InitializePreviewController(BuildingPrefabData buildingPrefabData)
        {
            var previewManager = new PreviewManager(this, buildingPrefabData.Prefab);
            return new PreviewController(this, buildingPrefabData, previewManager, initialSpawnPoint);
        }

        private void InitializeGrid()
        {
            var calculator = gameObject.AddComponent<GridCalculator>();
            calculator.Initialize(meshFilter, cellPerUnit);
            (GridSizeX, GridSizeZ, MinScaled, MaxScaled, GridWidth) =
                calculator.CalculateGridSize(transform.lossyScale);
        }

        private T GetOrAddComponent<T>() where T : Component
        {
            return GetComponent<T>() ?? gameObject.AddComponent<T>();
        }

        private void OnDestroy()
        {
            if (gridObjectSelector != null)
                gridObjectSelector.ObjectSelect -= OnObjectSelected;

            if (buildingManipulator != null)
                buildingManipulator.BuildingCompleted -= OnBuildingCompleted;
        }

    }
}