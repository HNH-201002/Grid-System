using UnityEngine;

namespace GridSystem.Core
{
    public class FootprintGenerator
    {
        private const float footprintSize = 1;
        private static Material footprintMaterial;

        public GameObject Generate()
        {
            GameObject footprintObj = new GameObject("BuildingFootprint");
            footprintObj.transform.localPosition = Vector3.zero;

            MeshFilter filter = footprintObj.AddComponent<MeshFilter>();
            filter.mesh = CreateFootprintMesh(footprintSize, footprintSize);

            MeshRenderer renderer = footprintObj.AddComponent<MeshRenderer>();
            renderer.material = GetOrCreateFootprintMaterial();

            footprintObj.SetActive(false);
            return footprintObj;
        }

        private Mesh CreateFootprintMesh(float xSize, float zSize)
        {
            Mesh mesh = new Mesh();

            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(-xSize / 2, 0, -zSize / 2),
                new Vector3(xSize / 2, 0, -zSize / 2),
                new Vector3(-xSize / 2, 0, zSize / 2),
                new Vector3(xSize / 2, 0, zSize / 2)
            };

            int[] triangles = new int[6]
            {
                0, 2, 1,
                2, 3, 1
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        private Material GetOrCreateFootprintMaterial()
        {
            if (footprintMaterial == null)
            {
                footprintMaterial = new Material(Shader.Find("Standard"))
                {
                    color = Color.green
                };
            }
            return footprintMaterial;
        }
    }
}
