using UnityEngine;
using GridSystem.Visualization;
using System;

namespace GridSystem.Core
{
    public class BuildingManipulator
    {
        private Building building;
        private PlacementPreview placementPreview;
        private int currentRotationAngle;

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
            RotateBuilding(-90);
        }

        public void RotateRight()
        {
            RotateBuilding(90);
        }

        private void RotateBuilding(int angle)
        {
            currentRotationAngle += angle;
            if (placementPreview != null)
            {
                placementPreview.transform.rotation = Quaternion.Euler(0, currentRotationAngle, 0);
            }
            else if (building != null)
            {
                building.transform.rotation = Quaternion.Euler(0, currentRotationAngle, 0);
            }
        }

        public void DeleteBuilding()
        {
            if (building != null)
            {
                GridManager.Instance.RemoveBuilding(building);
                building = null;
            }
        }

        public void AcceptPlacement()
        {
            if (placementPreview == null)
                return;

            if (!placementPreview.isActiveAndEnabled)
            {
                CleanupPlacement();
                return;
            }

            if (placementPreview.Build())
            {
                CleanupPlacement();
                BuildingCompleted?.Invoke();
            }
        }

        private void CleanupPlacement()
        {
            placementPreview = null;
            building = null;
        }
    }
}
