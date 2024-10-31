using UnityEngine;
using GridSystem.InputManagement;
using GridSystem.Data;
using System;
using GridSystem.UIManager;
using GridSystem.Visualization;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private GridSettings gridSettings;

        public GridSettings GridSettings => gridSettings;

        [SerializeField]
        private MeshFilter meshFilter;
        private Camera mainCamera;

        private Vector3 initialSpawnPoint;

        public static GridManager Instance { get; private set; }

        public float GridSizeX { get; private set; }
        public float GridSizeZ { get; private set; }

        public int GridWidth { get; private set; }

        public Vector3 MinScaled { get; private set; }
        public Vector3 MaxScaled { get; private set; }

        public GridState CurrentGameState { get; private set; }


        private IBuildingManager buildingManager;

        private IGridBehavior gridBehavior;

        private InputHandler inputHandler;

        private GridObjectSelector gridObjectSelector;

        private BuildingSelectorManager buildingTypeManager;

        private BuildingManipulator buildingManipulator;

        public event Action<Building> ObjectSelected;

        public event Action BuildCompleted;

        private void Awake()
        {
            gameObject.transform.localScale = new Vector3(gridSettings.GridSizeX, 0.1f, gridSettings.GridSizeZ);
            mainCamera = Camera.main;

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
            SetGameState(GridState.None);
            gridBehavior.GenerateGrid(MinScaled, MaxScaled, GridSizeX, GridSizeZ);
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
            buildingManager.Initialize(gridSettings.BuildingDataList, gridBehavior);
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

        public bool TryPlacePrefabOnGrid(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider)
        {
            return gridBehavior.TryPlaceBuilding(position, rotation, buildingType, boxCollider);
        }

        public Vector3 GetGridPosition(int gridIndex)
        {
            return gridBehavior.GetGridWorldPosition(gridIndex);
        }

        public void RemoveBuilding(Building building)
        {
            buildingManager.Remove(building);
        }

        public void SetGameState(GridState newGameState)
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
            initialSpawnPoint = gridBehavior.GetGridWorldPosition(gridBehavior.GetGridIndex(gridSettings.InitialSpawnPoint));
            var previewManager = new PreviewManager(this, gridBehavior, buildingPrefabData.Prefab);
            return new PreviewController(this, buildingPrefabData, previewManager, initialSpawnPoint);
        }

        public bool IsBoundsValid(Bounds bounds)
        {
            return gridBehavior.IsBoundsValid(bounds);
        }

        private void InitializeGrid()
        {
            var calculator = gameObject.AddComponent<GridCalculator>();
            calculator.Initialize(meshFilter, gridSettings.CellPerUnit);
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