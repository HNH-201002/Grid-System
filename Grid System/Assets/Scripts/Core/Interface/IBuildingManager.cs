using System;
using System.Collections.Generic;
using GridSystem.Data;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    /// <summary>
    /// Interface defining the behavior for building management.
    /// Provides methods to initialize, build, remove, and rotate buildings.
    /// </summary>
    public interface IBuildingManager
    {
        /// <summary>
        /// Initializes the building manager with a list of building data and grid behavior.
        /// </summary>
        public void Initialize(List<BuildingPrefabData> buildingDataList, IGridBehavior gridBehavior);

        /// <summary>
        /// Builds a building of the specified type.
        /// </summary>
        /// <param name="buildingType">The type of building to create.</param>
        /// <returns>The created building object.</returns>
        public Building Build(BuildingType buildingType);

        /// <summary>
        /// Removes the specified building from the grid and returns it to the pool.
        /// </summary>
        /// <param name="building">The building to remove.</param>
        public void Remove(Building building);

        /// <summary>
        /// Rotates the specified building by a given angle.
        /// </summary>
        /// <param name="building">The building to rotate.</param>
        /// <param name="angle">The angle to rotate the building by.</param>
        public void Rotate(Building building, int angle);

        /// <summary>
        /// Event triggered when a building is removed.
        /// </summary>
        public event Action<Building> BuildingRemoved;
    }
}
