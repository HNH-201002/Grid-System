using System.Collections.Generic;
using GridSystem.Visualization;
using UnityEngine;

namespace GridSystem.Core
{
    public class GridBehavior : MonoBehaviour
    {
        [SerializeField]
        private BuildingManager buildingManager;

        private List<Grid> grids = new List<Grid>();
        private HashSet<int> occupiedIndexes = new HashSet<int>();
        private GridManager gridManager;

        private void Start()
        {
            gridManager = GridManager.Instance;
            DrawGrid(gridManager.MinScaled, gridManager.MaxScaled, gridManager.GridSizeX, gridManager.GridSizeZ);
        }

        private void DrawGrid(Vector3 minScaled, Vector3 maxScaled, float gridSizeX, float gridSizeZ)
        {
            float xLimit = maxScaled.x - gridSizeX / 2;
            float zLimit = maxScaled.z - gridSizeZ / 2;

            float centerOffsetX = gridSizeX / 2;
            float centerOffsetZ = gridSizeZ / 2;

            for (float x = minScaled.x; x < xLimit; x += gridSizeX)
            {
                for (float z = minScaled.z; z < zLimit; z += gridSizeZ)
                {
                    Vector3 position = new Vector3(x + centerOffsetX, 0.01f, z + centerOffsetZ);
                    Grid grid = new Grid(position);
                    grids.Add(grid);
                }
            }
        }

        public void PlacePrefabOnGrid(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider)
        {
            Bounds bounds = boxCollider.bounds;

            int xIndexCount;
            int zIndexCount;
            List<int> gridIndex;

            (xIndexCount, zIndexCount, gridIndex) = GetGridIndexesFromBounds(bounds);

            if (IsOccupied(gridIndex))
                return;

            Building building = buildingManager.Build(buildingType);
            building.transform.position = position;
            building.transform.rotation = rotation;

            float xSize = xIndexCount * gridManager.GridSizeX;
            float zSize = zIndexCount * gridManager.GridSizeZ;

            building.Initialize(xSize, zSize, gridIndex);

            SetOccupied(gridIndex, true, building);
        }

        private bool IsOccupied(List<int> indexes)
        {
            foreach (int index in indexes)
            {
                if (IsIndexOccupied(index))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsIndexOccupied(int index)
        {
            return occupiedIndexes.Contains(index);
        }

        public void SetOccupied(List<int> indexes, bool isOccupied, Building build = null)
        {
            foreach (int index in indexes)
            {
                if (index < 0 || index >= grids.Count)
                    continue;

                if (isOccupied)
                {
                    occupiedIndexes.Add(index);
                }
                else
                {
                    occupiedIndexes.Remove(index);
                }

                grids[index].BuiltObject = build;
            }
        }

        private (int xCount, int zCount, List<int> indexes) GetGridIndexesFromBounds(Bounds bounds)
        {
            List<int> indexes = new List<int>();

            Vector3 minScaled = gridManager.MinScaled;
            int gridWidth = gridManager.GridWidth;

            int xStartIndex = Mathf.FloorToInt((bounds.min.x - minScaled.x) / gridManager.GridSizeX);
            int zStartIndex = Mathf.FloorToInt((bounds.min.z - minScaled.z) / gridManager.GridSizeZ);

            int xEndIndex = Mathf.FloorToInt((bounds.max.x - minScaled.x) / gridManager.GridSizeX);
            int zEndIndex = Mathf.FloorToInt((bounds.max.z - minScaled.z) / gridManager.GridSizeZ);

            int xCount = xEndIndex - xStartIndex + 1;
            int zCount = zEndIndex - zStartIndex + 1;

            for (int xIndex = xStartIndex; xIndex <= xEndIndex; xIndex++)
            {
                for (int zIndex = zStartIndex; zIndex <= zEndIndex; zIndex++)
                {
                    int gridIndex = xIndex * gridWidth + zIndex;

                    if (gridIndex >= 0 && gridIndex < grids.Count)
                    {
                        indexes.Add(gridIndex);
                    }
                }
            }

            return (xCount, zCount, indexes);
        }


        public int GetGridIndexFromPosition(Vector3 position)
        {
            int xIndex = Mathf.FloorToInt((position.x - gridManager.MinScaled.x) / gridManager.GridSizeX);
            int zIndex = Mathf.FloorToInt((position.z - gridManager.MinScaled.z) / gridManager.GridSizeZ);

            int gridIndex = xIndex * gridManager.GridWidth + zIndex;

            if (xIndex < 0 || zIndex < 0 || gridIndex >= grids.Count)
            {
                Debug.LogWarning("Invalid grid index: " + gridIndex);
                return -1;
            }

            return gridIndex;
        }

        public Vector3 GetGridPosition(int gridIndex)
        {
            if (gridIndex >= 0 && gridIndex < grids.Count)
            {
                return grids[gridIndex].Position;
            }

            Debug.LogWarning("Grid index out of range: " + gridIndex);
            return Vector3.zero;
        }

        public Building GetBuiltObjectInGrid(Vector3 position)
        {
            int index = GetGridIndexFromPosition(position);

            if (IsIndexOccupied(index))
            {
                return grids[index].BuiltObject;
            }

            return null;
        }
    }
}