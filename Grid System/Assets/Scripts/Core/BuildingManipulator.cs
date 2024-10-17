using UnityEngine;
using GridSystem.Visualization;
using System;

namespace GridSystem.Core
{
    public class BuildingManipulator
    {
        private Building building;
        private PlacementPreview placementPreview;
        private int angle;

        public event Action BuildingCompleted;

        public void SetBuilding(Building building)
        {
            this.building = building;
        }

        public void SetPlacementPreview(PlacementPreview preview)
        {
            placementPreview = preview;
        }

        public void RotateLeft()
        {
            angle -= 90;
            ApplyRotation();
        }

        public void RotateRight()
        {
            angle += 90;
            ApplyRotation();
        }

        public void DeleteBuilding()
        {
            if (building != null)
            {
                if (GridManager.Instance.CurrentGameState == GameState.Building)
                {
                    building = null;
                    return;
                }
                GridManager.Instance.RemoveBuilding(building);
                building = null;
            }
        }

        public void AcceptPlacement()
        {
            if (placementPreview != null && placementPreview.Build())
            {
                placementPreview = null;
                BuildingCompleted?.Invoke();
            }
        }

        private void ApplyRotation()
        {
            if (placementPreview != null)
            {
                placementPreview.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            else if (building != null)
            {
                building.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }

}