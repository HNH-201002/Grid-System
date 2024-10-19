using UnityEngine;
using GridSystem.Pooling;
using GridSystem.Visualization;
using GridSystem.Data;
using GridSystem.UIManager;
using GridSystem.Core.Enum;

namespace GridSystem.Core
{
    /// <summary>
    /// Manages the lifecycle of building previews, including activation, hiding, and setting up the preview component.
    /// </summary>
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

        /// <summary>
        /// Activates the preview at the specified position.
        /// Retrieves the preview instance from the pool if it's not already active.
        /// </summary>
        /// <param name="position">The world position to place the preview.</param>
        /// <returns>The activated preview GameObject.</returns>
        public GameObject ActivatePreview(Vector3 position)
        {
            if (previewInstance == null)
            {
                previewInstance = previewPool.GetFromPool();
            }

            previewInstance.transform.position = position;
            isBuildable = !gridBehavior.IsIndexOccupied(gridBehavior.GetGridIndex(position));
            gridManager.SetGameState(GridState.Building);
            return previewInstance;
        }

        /// <summary>
        /// Hides the currently active preview and returns it to the object pool.
        /// Resets the game state and preview controller.
        /// </summary>
        public void HidePreview()
        {
            if (previewInstance != null)
            {
                previewPool.ReturnToPool(previewInstance);
                previewInstance = null;
                previewComponent = null;
                previewController.SetDeactivePreviewMode();
                gridManager.SetGameState(GridState.None);
            }
        }

        /// <summary>
        /// Sets up the preview component with the given controller and building data.
        /// </summary>
        /// <param name="previewController">The PreviewController managing the preview state.</param>
        /// <param name="data">The BuildingPrefabData containing information about the building being previewed.</param>
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