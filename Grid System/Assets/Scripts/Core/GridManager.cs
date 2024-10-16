using UnityEngine;

namespace GridSystem.Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        [Tooltip("How many cells within 1 unit")]
        [SerializeField]
        private int cellPerUnit = 1;

        [SerializeField]
        private Material buildableMaterial;

        public Material BuildableMaterial => buildableMaterial;

        [SerializeField]
        private Material unbuildableMaterial;

        public Material UnBuildableMaterial => unbuildableMaterial;

        public static GridManager Instance { get; private set; }

        public float GridSizeX { get; private set; }
        public float GridSizeZ { get; private set; }

        public int GridWidth { get; private set; }

        public Vector3 MinScaled { get; private set; }
        public Vector3 MaxScaled { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                GridCalculator calculator = gameObject.AddComponent<GridCalculator>();
                calculator.Initialize(meshFilter, cellPerUnit);
                var (gridSizeX, gridSizeZ, minScaled, maxScaled, gridWidth) = calculator.CalculateGridSize(transform.lossyScale);

                GridSizeX = gridSizeX;
                GridSizeZ = gridSizeZ;
                MinScaled = minScaled;
                MaxScaled = maxScaled;
                GridWidth = gridWidth;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}