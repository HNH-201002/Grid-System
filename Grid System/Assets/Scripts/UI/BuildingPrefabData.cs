using UnityEngine;

namespace GridSystem.Data
{
    [CreateAssetMenu(fileName = "BuildingPrefabData", menuName = "ScriptableObjects/BuildingPrefabData", order = 1)]
    public class BuildingPrefabData : ScriptableObject
    {
        public GameObject Prefab;
        public BuildingType BuildingType;
    }
}
