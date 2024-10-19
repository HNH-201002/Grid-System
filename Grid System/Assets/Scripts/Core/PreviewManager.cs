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
        private IGridBehavior gridBehavior;
        private bool isBuildable = true;

        public PreviewManager(GridManager gridManager, IGridBehavior gridBehavior, GameObject prefab)
        {
            previewPool = new ObjectPool<GameObject>(prefab, 1);
            this.gridManager = gridManager;
            this.gridBehavior = gridBehavior;
        }

        public GameObject ActivatePreview(Vector3 position)
        {
            if (previewInstance == null)
            {
                previewInstance = previewPool.GetFromPool();
            }

            previewInstance.transform.position = position;
            isBuildable = !gridBehavior.IsIndexOccupied(gridBehavior.GetGridIndex(position));
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
            previewComponent.ClearPreviewState();
            previewComponent.ApplyMaterialToPreview(previewComponent.GetTargetMaterial(isBuildable));
            this.previewController = previewController;
            gridManager.SetPlacementPreviewInstance(previewComponent);
        }
    }
}