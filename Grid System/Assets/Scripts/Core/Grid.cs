using UnityEngine;

namespace GridSystem.Core
{
    /// <summary>
    /// Represents a single grid cell within the grid system.
    /// </summary>
    public class Grid
    {
        /// <summary>
        /// World position of the grid cell.
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// Building object currently placed on this grid cell.
        /// </summary>
        public Building BuiltObject { get; set; }

        public Grid(Vector3 position)
        {
            Position = position;
            BuiltObject = null;
        }
    }
}
