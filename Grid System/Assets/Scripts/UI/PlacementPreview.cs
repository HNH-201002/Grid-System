using UnityEngine;
using GridSystem.Core;
using GridSystem.Tags;
using GridSystem.InputManagement;
using GridSystem.Core.PhysicsCollider;
using GridSystem.Utilities;

namespace GridSystem.Visualization
{
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

        public bool PlaceBuildingFromPreview()
        {
            if (!collisionHandler.IsBuildable)
                return false;

            Vector3 position = gameObject.transform.position;
            Quaternion rotation = gameObject.transform.rotation;
            gridManager.PlacePrefabOnGrid(new Vector3(position.x, 0.001f, position.z), rotation, buildingType, boxCollider);
            ApplyMaterialToPreview(gridManager.UnBuildableMaterial);
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
            Material targetMaterial = isBuildable ? gridManager.BuildableMaterial : gridManager.UnBuildableMaterial;
            ApplyMaterialToPreview(targetMaterial);

            if (currentCellIndex != previousCellIndex && currentCellIndex != -1)
            {
                Vector3 gridPosition = gridManager.GetGridPosition(currentCellIndex);
                gameObject.transform.position = new Vector3(gridPosition.x, 0.5f, gridPosition.z);
                previousCellIndex = currentCellIndex;
            }
        }

        public Material GetTargetMaterial(bool isBuildable)
        {
            return isBuildable ? gridManager.BuildableMaterial : gridManager.UnBuildableMaterial;
        }

        public void ApplyMaterialToPreview(Material material)
        {
            if (meshRenderer != null && meshRenderer.material != material)
            {
                meshRenderer.material = material;
            }
        }

        public void ClearPreviewState()
        {
            previousCellIndex = -1;
            currentCellIndex = -1;

            ApplyMaterialToPreview(gridManager.BuildableMaterial);
        }
    }
}

