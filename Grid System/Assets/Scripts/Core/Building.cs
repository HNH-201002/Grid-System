using UnityEngine;
using System.Collections.Generic;
using GridSystem.Data;

namespace GridSystem.Core
{
    public class Building : MonoBehaviour
    {
        [SerializeField]
        private BuildingPrefabData buildingPrefabData;

        public BuildingPrefabData BuildingPrefabData => buildingPrefabData;

        private float xSize = 1.0f;
        private float zSize = 1.0f;

        private bool isToggled = false;

        public List<int> Indexes { get; private set; }

        public void ToggleFootprintDisplay(Transform footprint)
        {
            footprint.position = transform.position;
            footprint.localScale = new Vector3(xSize, 0.1f, zSize);
            isToggled = !isToggled;
            footprint.gameObject.SetActive(isToggled);
        }

        public void Initialize(float xSize, float zSize, List<int> indexes)
        {
            this.xSize = xSize;
            this.zSize = zSize;
            Indexes = indexes;
        }

        public void Reset()
        {
            isToggled = false;
        }
    }
}
