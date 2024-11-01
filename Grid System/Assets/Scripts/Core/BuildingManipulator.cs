using UnityEngine;
using GridSystem.Visualization;
using System;

namespace GridSystem.Core
{
    /// <summary>
    ///  Handles interactions between the grid system and building objects.
    /// </summary>
    public class BuildingManipulator
    {
        /// <summary>
        /// Event triggered when a building placement is completed.
        /// </summary>
        public event Action BuildingCompleted;

        private Building building;
        private PlacementPreview placementPreview;
        private int currentRotationAngle;
        private GridManager gridManager;

        public BuildingManipulator(GridManager gridManager)
        {
            this.gridManager = gridManager;
        }

        /// <summary>
        /// Sets the building object to be manipulated.
        /// </summary>
        /// <param name="building">The building object to manipulate.</param>
        public void SetBuilding(Building building)
        {
            this.building = building;
        }

        /// <summary>
        /// Sets the placement preview object for the building.
        /// </summary>
        /// <param name="preview">The placement preview object.</param>
        public void SetPlacementPreview(PlacementPreview preview)
        {
            placementPreview = preview;
        }

        /// <summary>
        /// Rotates the building or preview 90 degrees to the left.
        /// </summary>
        public void RotateLeft()
        {
            RotateBuilding(-90);
        }

        /// <summary>
        /// Rotates the building or preview 90 degrees to the right.
        /// </summary>
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

            placementPreview.UpdatePreviewAppearance();
        }

        /// <summary>
        /// Deletes the currently selected building from the grid.
        /// </summary>
        public void DeleteBuilding()
        {
            if (building != null)
            {
                gridManager.RemoveBuilding(building);
                building = null;
            }
        }

        /// <summary>
        /// Accepts the placement of the previewed building, finalizing its position on the grid.
        /// </summary>
        public void AcceptPlacement()
        {
            if (placementPreview == null)
                return;

            if (!placementPreview.isActiveAndEnabled)
            {
                CleanupPlacement();
                return;
            }

            if (placementPreview.PlaceBuildingFromPreview())
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
