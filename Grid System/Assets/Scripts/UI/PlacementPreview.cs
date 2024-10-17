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

        public void Initialize(PreviewManager previewManager, BuildingType buildingType)
        {
            this.buildingType = buildingType;
            this.previewManager = previewManager;
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
            previousMousePosition = inputHandler.GetInputPosition();
            collisionHandler.Initialize(GameTags.ConstructedObject);
            UpdatePreviewPosition(currentCellIndex, collisionHandler.IsBuildable);
            originalRotation = gameObject.transform.rotation;
        }

        private void OnMouseDrag()
        {
            if (inputHandler.HasInputMoved(previousMousePosition))
            {
                RaycastAndUpdateCellIndex();
                previousMousePosition = inputHandler.GetInputPosition();
            }

            UpdatePreviewPosition(currentCellIndex, collisionHandler.IsBuildable);
        }

        private void RaycastAndUpdateCellIndex()
        {
            Vector3? hit = raycastHandler.RaycastIfMoved(mainCamera, inputHandler.GetInputPosition());
            if (hit != null)
            {
                int newCellIndex = GridManager.Instance.GetGridIndexFromPosition(hit.Value);
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
            GridManager.Instance.PlacePrefabOnGrid(new Vector3(position.x, 0.001f, position.z), rotation, buildingType, boxCollider);
            SetMeshMaterial(gridManager.UnBuildableMaterial);
            gameObject.SetActive(false);

            previewManager.HidePreview();
            return true;
        }

        private void OnEnable()
        {
            transform.rotation = originalRotation;
        }

        private void UpdatePreviewPosition(int currentCellIndex, bool isBuildable)
        {
            Material targetMaterial = isBuildable ? gridManager.BuildableMaterial : gridManager.UnBuildableMaterial;
            SetMeshMaterial(targetMaterial);

            if (currentCellIndex != previousCellIndex && currentCellIndex != -1)
            {
                Vector3 gridPosition = GridManager.Instance.GetGridPosition(currentCellIndex);
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

