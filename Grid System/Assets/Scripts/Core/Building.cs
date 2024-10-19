using UnityEngine;
using System.Collections.Generic;
using GridSystem.Data;

namespace GridSystem.Core
{
    /// <summary>
    /// Represents a building within the grid system.
    /// Manages building data, footprint display, and maintains information about its grid placement.
    /// </summary>
    public class Building : MonoBehaviour
    {
        [SerializeField]
        private BuildingPrefabData buildingPrefabData;

        /// <summary>
        /// Gets the data associated with the building prefab.
        /// </summary>
        public BuildingPrefabData BuildingPrefabData => buildingPrefabData;

        private float xSize = 1.0f;
        private float zSize = 1.0f;
        private bool isToggled = false;

        /// <summary>
        /// Gets the list of grid indexes that this building occupies.
        /// </summary>
        public List<int> Indexes { get; private set; }

        /// <summary>
        /// Toggles the visibility of the building's footprint display.
        /// Updates the position and scale of the footprint transform.
        /// </summary>
        /// <param name="footprint">The transform representing the footprint to display.</param>
        public void ToggleFootprintDisplay(Transform footprint)
        {
            footprint.position = transform.position;
            footprint.localScale = new Vector3(xSize, 0.1f, zSize);
            isToggled = !isToggled;
            footprint.gameObject.SetActive(isToggled);
        }

        /// <summary>
        /// Initializes the building with size and grid index information.
        /// </summary>
        /// <param name="xSize">The size of the building along the X-axis.</param>
        /// <param name="zSize">The size of the building along the Z-axis.</param>
        /// <param name="indexes">The list of grid indexes occupied by the building.</param>
        public void Initialize(float xSize, float zSize, List<int> indexes)
        {
            this.xSize = xSize;
            this.zSize = zSize;
            Indexes = indexes;
        }

        /// <summary>
        /// Resets the building's state, turning off any toggled displays.
        /// </summary>
        public void Reset()
        {
            isToggled = false;
        }
    }
}
