using UnityEngine;

namespace GridSystem.Core
{
    public class GridCalculator : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private int cellPerUnit;

        public void Initialize(MeshFilter meshFilter, int cellPerUnit)
        {
            this.meshFilter = meshFilter;
            this.cellPerUnit = cellPerUnit;
        }

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
