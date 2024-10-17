using UnityEngine;
using GridSystem.Pooling;
using GridSystem.Visualization;
using GridSystem.Data;
using GridSystem.UIManager;

namespace GridSystem.Core
{
    public class PreviewManager
    {
        private BuildingPreviewToggle buildingPreviewToggle;
        private ObjectPool<GameObject> previewPool;
        private GameObject previewInstance;
        private PlacementPreview previewComponent;

        public PreviewManager(GameObject prefab)
        {
            previewPool = new ObjectPool<GameObject>(prefab, 1);
        }

        public GameObject ActivatePreview(Vector3 position)
        {
            if (previewInstance == null)
            {
                previewInstance = previewPool.GetFromPool();
                previewInstance.transform.position = position;
            }

            return previewInstance;
        }

        public void HidePreview()
        {
            if (previewInstance != null)
            {
                previewPool.ReturnToPool(previewInstance);
                previewInstance = null;
                previewComponent = null;
                buildingPreviewToggle.SetDeactivePreviewMode();
                GridManager.Instance.SetGameState(GameState.None);
            }
        }

        public void SetupPreviewComponent(BuildingPreviewToggle buildingPreviewToggle, BuildingPrefabData data)
        {
            previewComponent = previewInstance.GetComponent<PlacementPreview>() ??
                               previewInstance.AddComponent<PlacementPreview>();

            previewComponent.Initialize(this, data.BuildingType);
            previewComponent.ResetPreview();
            this.buildingPreviewToggle = buildingPreviewToggle;
            GridManager.Instance.SetPlacementPreviewInstance(previewComponent);
        }
    }
}