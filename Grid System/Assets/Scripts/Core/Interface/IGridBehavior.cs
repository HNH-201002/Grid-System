using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem.Core
{
    public interface IGridBehavior
    {
        public void Initialize(GridManager gridManager, IBuildingManager buildingManager);
        public void PlaceBuilding(Vector3 position, Quaternion rotation, BuildingType buildingType, BoxCollider boxCollider);
        public int GetGridIndex(Vector3 position);
        public Vector3 GetGridWorldPosition(int gridIndex);
        public void SetGridOccupied(List<int> indexes, bool isOccupied, Building build = null);
        public bool IsIndexOccupied(int index);
        public Building GetBuildingAtGrid(Vector3 position);
    }
}
