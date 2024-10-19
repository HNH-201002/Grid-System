using UnityEngine;
using System.Collections.Generic;

namespace GridSystem.Data
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "GridSystem/Grid Settings", order = 1)]
    public class GridSettings : ScriptableObject
    {
        [Tooltip("Number of cells per unit")]
        public int CellPerUnit = 1;

        [Tooltip("Grid size along the X-axis")]
        public float GridSizeX = 10f;

        [Tooltip("Grid size along the Z-axis")]
        public float GridSizeZ = 10f;

        [Tooltip("Material used for buildable areas")]
        public Material BuildableMaterial;

        [Tooltip("Material used for unbuildable areas")]
        public Material UnbuildableMaterial;

        [Tooltip("Initial spawn position for buildings")]
        public Vector3 InitialSpawnPoint = new Vector3(0, 0.5f, 0);

        [Tooltip("List of building prefabs used in the grid system")]
        public List<BuildingPrefabData> BuildingDataList;
    }
}
