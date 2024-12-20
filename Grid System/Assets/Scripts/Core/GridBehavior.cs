using System.Collections.Generic;
using UnityEngine;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    /// <summary>
    /// Manages the behavior of the grid, including grid generation, placement of buildings,
    /// and tracking of occupied grid cells. 
    /// </summary>
    public class GridBehavior : MonoBehaviour, IGridBehavior
    {
        private IBuildingManager buildingManager;
        private List<Grid> grids = new List<Grid>();
        private HashSet<int> occupiedIndexes = new HashSet<int>();
        private GridManager gridManager;

        /// <inheritdoc/>        
        public void Initialize(GridManager gridManager, IBuildingManager buildingManager)
        {
            this.buildingManager = buildingManager;
            this.gridManager = gridManager;
        }

        /// <inheritdoc/>        
        public void GenerateGrid(Vector3 minScaled, Vector3 maxScaled, float gridSizeX, float gridSizeZ)
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
                    grids.Add(new Grid(position));
                }
            }
        }

        /// <inheritdoc/>
        public bool TryPlaceBuilding(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider)
        {
            Bounds bounds = boxCollider.bounds;

            if (!IsBoundsValid(bounds))
                return false;

            int xIndexCount;
            int zIndexCount;
            List<int> gridIndexes;

            (xIndexCount, zIndexCount, gridIndexes) = GetGridIndexesFromBounds(bounds);

            if (gridIndexes.Count <= 0)
                return false;

            if (AreIndexesOccupied(gridIndexes))
                return false;

            Building building = buildingManager.Build(buildingType);
            building.transform.position = position;
            building.transform.rotation = rotation;

            float xSize = xIndexCount * gridManager.GridSizeX + 1;
            float zSize = zIndexCount * gridManager.GridSizeZ + 1;

            xSize = xSize % 2 == 0 ? xSize + 1 : xSize;
            zSize = zSize % 2 == 0 ? zSize + 1 : zSize;

            building.Initialize(xSize, zSize, gridIndexes);

            SetGridOccupied(gridIndexes, true, building);

            return true;
        }

        private bool AreIndexesOccupied(List<int> indexes)
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

        /// <inheritdoc/>
        public bool IsIndexOccupied(int index)
        {
            if (index < 0) return false;

            return occupiedIndexes.Contains(index);
        }

        /// <inheritdoc/>
        public void SetGridOccupied(List<int> indexes, bool isOccupied, Building build = null)
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

        public bool IsBoundsValid(Bounds bounds)
        {
            Vector3 minScaled = gridManager.MinScaled;
            int gridWidth = gridManager.GridWidth;
            int gridHeight = gridManager.GridHeight;

            int xStartIndex = Mathf.FloorToInt((bounds.min.x - minScaled.x) / gridManager.GridSizeX);
            int zStartIndex = Mathf.FloorToInt((bounds.min.z - minScaled.z) / gridManager.GridSizeZ);

            int xEndIndex = Mathf.FloorToInt((bounds.max.x - minScaled.x) / gridManager.GridSizeX);
            int zEndIndex = Mathf.FloorToInt((bounds.max.z - minScaled.z) / gridManager.GridSizeZ);

            int gridIndexMax = xEndIndex * gridWidth + zEndIndex;
            int gridIndexMin = xStartIndex * gridWidth + zStartIndex;

            return !(xStartIndex < 0 || zStartIndex < 0 || xEndIndex >= gridWidth || zEndIndex >= gridHeight || gridIndexMin < 0 || gridIndexMax >= grids.Count);
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

            int xCount = xEndIndex - xStartIndex;
            int zCount = zEndIndex - zStartIndex;

            for (int xIndex = xStartIndex; xIndex <= xEndIndex; xIndex++)
            {
                for (int zIndex = zStartIndex; zIndex <= zEndIndex; zIndex++)
                {
                    int gridIndex = xIndex * gridWidth + zIndex;

                    indexes.Add(gridIndex);
                }
            }

            return (xCount, zCount, indexes);
        }

        /// <inheritdoc/>
        public int GetGridIndex(Vector3 position)
        {
            int xIndex = Mathf.FloorToInt((position.x - gridManager.MinScaled.x) / gridManager.GridSizeX);
            int zIndex = Mathf.FloorToInt((position.z - gridManager.MinScaled.z) / gridManager.GridSizeZ);

            int gridIndex = xIndex * gridManager.GridWidth + zIndex;

            if (xIndex < 0 || zIndex < 0 || gridIndex >= grids.Count)
            {
                return -1;
            }

            return gridIndex;
        }

        /// <inheritdoc/>
        public Vector3 GetGridWorldPosition(int gridIndex)
        {
            if (gridIndex >= 0 && gridIndex < grids.Count)
            {
                return grids[gridIndex].Position;
            }

            return grids[GetGridIndex(Vector3.zero)].Position;
        }

        /// <inheritdoc/>
        public Building GetBuildingAtGrid(Vector3 position)
        {
            int index = GetGridIndex(position);

            if (IsIndexOccupied(index))
            {
                return grids[index].BuiltObject;
            }

            return null;
        }
    }
}
