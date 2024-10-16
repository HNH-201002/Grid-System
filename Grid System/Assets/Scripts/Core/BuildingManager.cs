
using System;
using System.Collections.Generic;
using GridSystem.Data;
using GridSystem.Pooling;
using UnityEngine;

namespace GridSystem.Core
{
    public class BuildingManager : MonoBehaviour, IBuildingManager
    {
        [SerializeField]
        private List<BuildingPrefabData> buildingDataList;

        private Dictionary<BuildingType, GenericObjectPool<Building>> poolDictionary;

        [SerializeField]
        private GridBehavior gridBehavior;

        public event Action<Building> BuildingRemoved;

        private void Start()
        {
            poolDictionary = new Dictionary<BuildingType, GenericObjectPool<Building>>();

            foreach (var buildingData in buildingDataList)
            {
                var pool = new GenericObjectPool<Building>(buildingData.Prefab.GetComponent<Building>(), 0);
                poolDictionary.Add(buildingData.BuildingType, pool);
            }
        }

        public void Remove(Building building)
        {
            BuildingRemoved?.Invoke(building);
            List<int> indexes = building.Indexes;
            gridBehavior.SetOccupied(indexes, false);
            poolDictionary[building.BuildingPrefabData.BuildingType].ReturnToPool(building);
        }

        public Building Build(BuildingType buildingType)
        {
            var building = poolDictionary[buildingType].GetFromPool();
            return building;
        }

        public void Rotate(Building building, int angle)
        {
            building.transform.rotation = Quaternion.Euler(0, building.transform.rotation.eulerAngles.y + angle, 0);
        }
    }
}