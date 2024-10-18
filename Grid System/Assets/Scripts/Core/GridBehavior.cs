using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GridSystem.Core
{
    public class GridBehavior : MonoBehaviour
    {
        private BuildingManager buildingManager;
        private List<Grid> grids = new List<Grid>();
        private HashSet<int> occupiedIndexes = new HashSet<int>();
        private GridManager gridManager;

        public void Initialize(BuildingManager buildingManager)
        {
            this.buildingManager = buildingManager;
        }

        private void Start()
        {
            gridManager = GridManager.Instance;
            StartCoroutine(DrawGrid(gridManager.MinScaled, gridManager.MaxScaled, gridManager.GridSizeX, gridManager.GridSizeZ));
        }

        private IEnumerator DrawGrid(Vector3 minScaled, Vector3 maxScaled, float gridSizeX, float gridSizeZ)
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
                    if (Time.frameCount % 20 == 0)
                        yield return null;
                }
            }
        }

        public async Task PlacePrefabOnGrid(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider)
        {
            Bounds bounds = boxCollider.bounds;

            var (xIndexCount, zIndexCount, gridIndex) = await GetGridIndexesFromBoundsAsync(bounds);

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

        private async Task<(int xCount, int zCount, List<int> indexes)> GetGridIndexesFromBoundsAsync(Bounds bounds)
        {
            return await Task.Run(() =>
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
            });
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
