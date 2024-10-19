using UnityEngine;

namespace GridSystem.Core
{
    /// <summary>
    /// It computes the dimensions based on the mesh filter, scale, and the number of cells per unit.
    /// </summary>
    public class GridCalculator : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private int cellPerUnit;

        public void Initialize(MeshFilter meshFilter, int cellPerUnit)
        {
            this.meshFilter = meshFilter;
            this.cellPerUnit = cellPerUnit;
        }

        /// <summary>
        /// Calculates the grid size, scaled boundaries, and grid width based on the mesh and scale.
        /// </summary>
        /// <param name="scale">The scale of the grid system.</param>
        /// <returns>
        /// A tuple containing:
        /// - gridSizeX: The width of each grid cell along the X-axis.
        /// - gridSizeZ: The depth of each grid cell along the Z-axis.
        /// - minScaled: The minimum scaled boundary of the grid.
        /// - maxScaled: The maximum scaled boundary of the grid.
        /// - gridWidth: The total width of the grid in terms of cells.
        /// </returns>
        public (float gridSizeX, float gridSizeZ, Vector3 minScaled, Vector3 maxScaled, int gridWidth) CalculateGridSize(Vector3 scale)
        {
            Bounds bounds = meshFilter.sharedMesh.bounds;

            Vector3 minScaled = Multiply(bounds.min, scale);
            Vector3 maxScaled = Multiply(bounds.max, scale);

            float gridSizeX = maxScaled.x / cellPerUnit;
            float gridSizeZ = maxScaled.z / cellPerUnit;

            float minGridSize = 0.1f;
            gridSizeX = Mathf.Max(gridSizeX, minGridSize);
            gridSizeZ = Mathf.Max(gridSizeZ, minGridSize);
            int gridWidth = Mathf.FloorToInt((meshFilter.sharedMesh.bounds.size.x * transform.lossyScale.x) / gridSizeX);
            return (gridSizeX, gridSizeZ, minScaled, maxScaled, gridWidth);
        }

        private Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }

}
