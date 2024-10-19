using System;
using UnityEngine;

namespace GridSystem.Visualization
{
    /// <summary>
    /// Visualizes a grid overlay in the Unity editor using Gizmos.
    /// The grid adapts based on the object's scale and mesh bounds, allowing for customized cell sizes.
    /// </summary>
    public class GridOverlay : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [Tooltip("How many cells within 1 unit")]
        [SerializeField]
        private int cellPerUnit = 1;

        private void OnDrawGizmos()
        {
            if (!ValidateInputs())
                return;

            Vector3 scale = transform.lossyScale;
            Bounds bounds = meshFilter.sharedMesh.bounds;

            Gizmos.color = Color.red;

            Vector3 minScaled = Multiply(bounds.min, scale);
            Vector3 maxScaled = Multiply(bounds.max, scale);

            float gridSizeX = maxScaled.x / cellPerUnit;
            float gridSizeZ = maxScaled.z / cellPerUnit;

            float minGridSize = 0.1f;
            gridSizeX = Mathf.Max(gridSizeX, minGridSize);
            gridSizeZ = Mathf.Max(gridSizeZ, minGridSize);

            DrawGrid(minScaled, maxScaled, gridSizeX, gridSizeZ);
        }

        private bool ValidateInputs()
        {
            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.LogError("MeshFilter or Mesh is not assigned.");
                return false;
            }

            if (cellPerUnit <= 0)
            {
                Debug.LogError("Please modify the cellPerUnit value to be greater than 0.");
                return false;
            }

            Vector3 scale = transform.lossyScale;

            if (scale.x != scale.z)
            {
                Debug.LogError("Please modify the scale of x-axis and z-axis to be equal.");
                return false;
            }

            return true;
        }

        private void DrawGrid(Vector3 minScaled, Vector3 maxScaled, float gridSizeX, float gridSizeZ)
        {
            for (float x = minScaled.x; x + gridSizeX / 2 < maxScaled.x; x += gridSizeX)
            {
                for (float z = minScaled.z; z + gridSizeZ / 2 < maxScaled.z; z += gridSizeZ)
                {
                    Gizmos.DrawLine(new Vector3(x, 0, z), new Vector3(x, 0, z + gridSizeZ));
                    Gizmos.DrawLine(new Vector3(x, 0, z), new Vector3(x + gridSizeX, 0, z));
                }
            }

            Gizmos.DrawLine(new Vector3(maxScaled.x, 0, minScaled.z), new Vector3(maxScaled.x, 0, maxScaled.z));
            Gizmos.DrawLine(new Vector3(minScaled.x, 0, maxScaled.z), new Vector3(maxScaled.x, 0, maxScaled.z));
        }

        /// <summary>
        /// Multiplies two vectors element-wise.
        /// </summary>
        public Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
    }
}
