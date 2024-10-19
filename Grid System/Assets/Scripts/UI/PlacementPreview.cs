using UnityEngine;
using GridSystem.Core;
using GridSystem.Tags;
using GridSystem.InputManagement;
using GridSystem.Core.PhysicsCollider;
using GridSystem.Utilities;

namespace GridSystem.Visualization
{
    /// <summary>
    /// Updates the appearance of the preview based on whether the area is buildable or not.
    /// </summary>
    public class PlacementPreview : MonoBehaviour
    {
        private MeshRenderer meshRenderer;
        private InputHandler inputHandler;
        private CollisionHandler collisionHandler;
        private RaycastHandler raycastHandler = new RaycastHandler();
        private BoxCollider boxCollider;
        private int previousCellIndex = -1;
        private int currentCellIndex = -1;
        private BuildingType buildingType;
        private Vector3 previousMousePosition;
        private GridManager gridManager;

        private Camera mainCamera;

        private PreviewManager previewManager;

        private Quaternion originalRotation;

        public void Initialize(GridManager gridManager, PreviewManager previewManager, BuildingType buildingType)
        {
            this.buildingType = buildingType;
            this.previewManager = previewManager;
            this.gridManager = gridManager;
        }

        private void Start()
        {
            mainCamera = Camera.main;

            if (gameObject.TryGetComponent(out BoxCollider boxCollider))
            {
                this.boxCollider = boxCollider;
            }
            else
            {
                Debug.LogError("BoxCollider not assigned to placement preview. Please attach BoxCollider!");
            }

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            inputHandler = gameObject.AddComponent<InputHandler>();
            collisionHandler = gameObject.AddComponent<CollisionHandler>();
            previousMousePosition = inputHandler.GetInputPosition();
            collisionHandler.Initialize(GameTags.ConstructedObject);
            UpdatePreviewAppearance(currentCellIndex, collisionHandler.IsBuildable);
            originalRotation = gameObject.transform.rotation;
            inputHandler.OnInputHold += CheckInputMovement;
        }

        private void OnDestroy()
        {
            inputHandler.OnInputHold -= CheckInputMovement;
        }

        private void CheckInputMovement()
        {
            if (inputHandler.HasInputMoved(previousMousePosition))
            {
                RaycastAndUpdateCellIndex();
                previousMousePosition = inputHandler.GetInputPosition();
            }

            UpdatePreviewAppearance(currentCellIndex, collisionHandler.IsBuildable);
        }

        private void RaycastAndUpdateCellIndex()
        {
            Vector3? hit = raycastHandler.RaycastIfMoved(mainCamera, inputHandler.GetInputPosition());
            if (hit != null)
            {
                int newCellIndex = gridManager.GetGridIndexFromPosition(hit.Value);
                if (newCellIndex != currentCellIndex)
                {
                    currentCellIndex = newCellIndex;
                }
            }
        }

        /// <summary>
        /// Places the building on the grid if the area is buildable.
        /// </summary>
        /// <returns>True if the building was placed successfully, false otherwise.</returns>
        public bool PlaceBuildingFromPreview()
        {
            if (!collisionHandler.IsBuildable)
                return false;

            Vector3 position = gameObject.transform.position;
            Quaternion rotation = gameObject.transform.rotation;
            gridManager.PlacePrefabOnGrid(new Vector3(position.x, 0.001f, position.z), rotation, buildingType, boxCollider);
            ApplyMaterialToPreview(gridManager.GridSettings.UnbuildableMaterial);
            gameObject.SetActive(false);

            previewManager.HidePreview();
            return true;
        }

        private void OnEnable()
        {
            transform.rotation = originalRotation;
        }

        private void UpdatePreviewAppearance(int currentCellIndex, bool isBuildable)
        {
            Material targetMaterial = GetTargetMaterial(isBuildable);
            ApplyMaterialToPreview(targetMaterial);

            if (currentCellIndex != previousCellIndex && currentCellIndex != -1)
            {
                Vector3 gridPosition = gridManager.GetGridPosition(currentCellIndex);
                gameObject.transform.position = new Vector3(gridPosition.x, 0.5f, gridPosition.z);
                previousCellIndex = currentCellIndex;
            }
        }

        /// <summary>
        /// Gets the appropriate material based on whether the area is buildable.
        /// </summary>
        /// <param name="isBuildable">Whether the current area is buildable.</param>
        /// <returns>The material to apply to the preview object.</returns>
        public Material GetTargetMaterial(bool isBuildable)
        {
            return isBuildable ? gridManager.GridSettings.BuildableMaterial : gridManager.GridSettings.UnbuildableMaterial;
        }

        /// <summary>
        /// Applies the given material to the preview object.
        /// </summary>
        /// <param name="material">The material to apply.</param>
        public void ApplyMaterialToPreview(Material material)
        {
            if (meshRenderer != null && meshRenderer.material != material)
            {
                meshRenderer.material = material;
            }
        }

        /// <summary>
        /// Clears the state of the preview, resetting cell indices and applying the buildable material.
        /// </summary>
        public void ClearPreviewState()
        {
            previousCellIndex = -1;
            currentCellIndex = -1;

            ApplyMaterialToPreview(gridManager.GridSettings.BuildableMaterial);
        }
    }
}

