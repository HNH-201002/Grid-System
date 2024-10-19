using UnityEngine;
using GridSystem.Pooling;
using GridSystem.Visualization;
using GridSystem.Data;
using GridSystem.UIManager;

namespace GridSystem.Core
{
    public class PreviewManager
    {
        private PreviewController previewController;
        private ObjectPool<GameObject> previewPool;
        private GameObject previewInstance;
        private PlacementPreview previewComponent;
        private GridManager gridManager;

        public PreviewManager(GridManager gridManager, GameObject prefab)
        {
            previewPool = new ObjectPool<GameObject>(prefab, 1);
            this.gridManager = gridManager;
        }

        public GameObject ActivatePreview(Vector3 position)
        {
            if (previewInstance == null)
            {
                previewInstance = previewPool.GetFromPool();
            }

            previewInstance.transform.position = position;
            gridManager.SetGameState(GameState.Building);
            return previewInstance;
        }

        public void HidePreview()
        {
            if (previewInstance != null)
            {
                previewPool.ReturnToPool(previewInstance);
                previewInstance = null;
                previewComponent = null;
                previewController.SetDeactivePreviewMode();
                gridManager.SetGameState(GameState.None);
            }
        }

        public void SetupPreviewComponent(PreviewController previewController, BuildingPrefabData data)
        {
            previewComponent = previewInstance.GetComponent<PlacementPreview>() ??
                               previewInstance.AddComponent<PlacementPreview>();

            previewComponent.Initialize(gridManager, this, data.BuildingType);
            previewComponent.ResetPreview();
            this.previewController = previewController;
            gridManager.SetPlacementPreviewInstance(previewComponent);
        }
    }
}