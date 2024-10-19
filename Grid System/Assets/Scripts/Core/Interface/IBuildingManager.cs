using System;
using System.Collections.Generic;
using GridSystem.Data;

namespace GridSystem.Core
{
    public interface IBuildingManager
    {
        public void Initialize(List<BuildingPrefabData> buildingDataList, IGridBehavior gridBehavior);
        public Building Build(BuildingType buildingType);
        public void Remove(Building building);
        public void Rotate(Building building, int angle);
        public event Action<Building> BuildingRemoved;
    }

}