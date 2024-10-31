using System.Collections.Generic;
using UnityEngine;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    /// <summary>
    /// Defines the behavior and interactions for a grid system.
    /// Provides methods for initializing the grid, placing buildings, and managing grid states.
    /// </summary>
    public interface IGridBehavior
    {
        /// <summary>
        /// Initializes the grid behavior with references to the GridManager and BuildingManager.
        /// </summary>
        /// <param name="gridManager">The GridManager that controls the grid system.</param>
        /// <param name="buildingManager">The BuildingManager that manages buildings within the grid.</param>
        public void Initialize(GridManager gridManager, IBuildingManager buildingManager);

        /// <summary>
        /// Generates the grid with specified dimensions and boundaries.
        /// </summary>
        /// <param name="minScaled">The minimum boundary of the grid in world coordinates.</param>
        /// <param name="maxScaled">The maximum boundary of the grid in world coordinates.</param>
        /// <param name="gridSizeX">The size of each cell along the X-axis.</param>
        /// <param name="gridSizeZ">The size of each cell along the Z-axis.</param>
        public void GenerateGrid(Vector3 minScaled, Vector3 maxScaled, float gridSizeX, float gridSizeZ);

        /// <summary>
        /// Places a building on the grid if the area is not occupied.
        /// </summary>
        public bool TryPlaceBuilding(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider);

        /// <summary>
        /// Gets the index of the grid cell corresponding to the given world position.
        /// </summary>
        /// <param name="position">The world position to query.</param>
        /// <returns>The index of the grid cell at the specified position.</returns>
        public int GetGridIndex(Vector3 position);

        /// <summary>
        /// Gets the world position of a grid cell by its index.
        /// </summary>
        /// <param name="gridIndex">The index of the grid cell.</param>
        /// <returns>The world position of the grid cell.</returns>
        public Vector3 GetGridWorldPosition(int gridIndex);

        /// <summary>
        /// Marks the specified grid cells as occupied or unoccupied.
        /// </summary>
        /// <param name="indexes">A list of grid cell indexes to update.</param>
        /// <param name="isOccupied">Whether the cells should be marked as occupied or unoccupied.</param>
        /// <param name="build">The building object occupying the cells (optional).</param>
        public void SetGridOccupied(List<int> indexes, bool isOccupied, Building build = null);

        /// <summary>
        /// Checks if a specific grid cell is occupied.
        /// </summary>
        /// <param name="index">The index of the grid cell to check.</param>
        /// <returns>True if the cell is occupied, false otherwise.</returns>
        public bool IsIndexOccupied(int index);

        /// <summary>
        /// Gets the building located at a specific world position, if any.
        /// </summary>
        /// <param name="position">The world position to query.</param>
        /// <returns>The building at the specified position, or null if none exists.</returns>
        public Building GetBuildingAtGrid(Vector3 position);

        /// <summary>
        /// Whether bounds is valid or not.
        /// </summary>
        public bool IsBoundsValid(Bounds bounds);
    }
}
