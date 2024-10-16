using UnityEngine;
using GridSystem.Core;
using GridSystem.UIManager;
using GridSystem.Tags;
using GridSystem.InputManagement;
using GridSystem.Core.PhysicsCollider;
using GridSystem.Utilities;
using Unity.VisualScripting;

namespace GridSystem.Visualization
{
    public class PlacementPreview : MonoBehaviour
    {
        private GridBehavior gridBehavior;
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

        private BuildingTypeSelector buildingTypeSelector;

        private Quaternion originalRotation;
        public void Initialize(GridBehavior gridBehavior, BuildingTypeSelector buildingTypeSelector, BuildingType buildingType)
        {
            this.gridBehavior = gridBehavior;
            this.buildingType = buildingType;
            this.buildingTypeSelector = buildingTypeSelector;
        }

        private void Start()
        {
            mainCamera = Camera.main;

            gridManager = GridManager.Instance;

            if (gameObject.TryGetComponent(out BoxCollider boxCollider))
            {
                this.boxCollider = boxCollider;
            }
            else
            {
                Debug.LogError("BoxCollider not assigned to placement preview. Please attach BoxCollider!");
            }

            meshRenderer = GetComponent<MeshRenderer>();
            inputHandler = gameObject.AddComponent<InputHandler>();
            collisionHandler = gameObject.AddComponent<CollisionHandler>();
            previousMousePosition = inputHandler.GetMousePosition();
            collisionHandler.Initialize(GameTags.ConstructedObject);
            UpdatePreviewPosition(currentCellIndex, collisionHandler.IsBuildable);
            originalRotation = gameObject.transform.rotation;
        }

        private void OnMouseDrag()
        {
            if (inputHandler.HasMouseMoved(previousMousePosition))
            {
                RaycastAndUpdateCellIndex();
                previousMousePosition = inputHandler.GetMousePosition();
            }

            UpdatePreviewPosition(currentCellIndex, collisionHandler.IsBuildable);
        }

        private void RaycastAndUpdateCellIndex()
        {
            Vector3? hit = raycastHandler.GetPositionFromRaycast(mainCamera, inputHandler.GetMousePosition());
            if (hit != null)
            {
                int newCellIndex = gridBehavior.GetGridIndexFromPosition(hit.Value);
                if (newCellIndex != currentCellIndex)
                {
                    currentCellIndex = newCellIndex;
                }
            }
        }

        public bool Build()
        {
            if (!collisionHandler.IsBuildable)
                return false;

            Vector3 position = gameObject.transform.position;
            Quaternion rotation = gameObject.transform.rotation;
            gridBehavior.PlacePrefabOnGrid(new Vector3(position.x, 0.001f, position.z), rotation, buildingType, boxCollider);
            SetMeshMaterial(gridManager.UnBuildableMaterial);
            gameObject.SetActive(false);

            buildingTypeSelector.HidePreviewInstance();
            return true;
        }

        private void OnEnable()
        {
            transform.rotation = originalRotation;
        }

        private void UpdatePreviewPosition(int currentCellIndex, bool isBuildable)
        {
            if (gridBehavior == null)
                return;

            Material targetMaterial = isBuildable ? gridManager.BuildableMaterial : gridManager.UnBuildableMaterial;
            SetMeshMaterial(targetMaterial);

            if (currentCellIndex != previousCellIndex && currentCellIndex != -1)
            {
                Vector3 gridPosition = gridBehavior.GetGridPosition(currentCellIndex);
                gameObject.transform.position = new Vector3(gridPosition.x, 0.5f, gridPosition.z);
                previousCellIndex = currentCellIndex;
            }
        }

        private void SetMeshMaterial(Material material)
        {
            if (meshRenderer != null && meshRenderer.material != material)
            {
                meshRenderer.material = material;
            }
        }

        public void ResetPreview()
        {
            transform.position = Vector3.zero;
            previousCellIndex = -1;
            currentCellIndex = -1;

            SetMeshMaterial(GridManager.Instance.BuildableMaterial);
        }
    }
}

