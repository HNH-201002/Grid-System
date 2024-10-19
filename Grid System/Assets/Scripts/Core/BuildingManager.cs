using System;
using System.Collections.Generic;
using GridSystem.Data;
using GridSystem.Pooling;
using UnityEngine;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    /// <summary>
    /// Manages the lifecycle of buildings within the grid system.
    /// Responsible for building, rotating, removing buildings, and managing object pools.
    /// </summary>
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        private List<BuildingPrefabData> buildingDataList;

        private Dictionary<BuildingType, GenericObjectPool<Building>> poolDictionary;

        private IGridBehavior gridBehavior;

        /// <inheritdoc/>
        public event Action<Building> BuildingRemoved;

        public void Initialize(List<BuildingPrefabData> buildingDataList, IGridBehavior gridBehavior)
        {
            this.buildingDataList = buildingDataList;
            this.gridBehavior = gridBehavior;
        }

        private void Start()
        {
            poolDictionary = new Dictionary<BuildingType, GenericObjectPool<Building>>();

            foreach (var buildingData in buildingDataList)
            {
                var pool = new GenericObjectPool<Building>(buildingData.Prefab.GetComponent<Building>(), 0);
                poolDictionary.Add(buildingData.BuildingType, pool);
            }
        }

        /// <inheritdoc/>
        public void Remove(Building building)
        {
            BuildingRemoved?.Invoke(building);
            List<int> indexes = building.Indexes;
            gridBehavior.SetGridOccupied(indexes, false);
            poolDictionary[building.BuildingPrefabData.BuildingType].ReturnToPool(building);
        }

        /// <inheritdoc/>
        public Building Build(BuildingType buildingType)
        {
            var building = poolDictionary[buildingType].GetFromPool();
            return building;
        }

        /// <inheritdoc/>
        public void Rotate(Building building, int angle)
        {
            building.transform.rotation = Quaternion.Euler(0, building.transform.rotation.eulerAngles.y + angle, 0);
        }
    }
}