using GridSystem.Core;
using GridSystem.Visualization;
using UnityEngine;
using UnityEngine.UI;

namespace GridSystem.UIManager
{
    public class ObjectInteractionUI : MonoBehaviour
    {
        [SerializeField]
        private GridObjectSelector gridObjectSelector;

        [SerializeField]
        private BuildingManager buildingManager;

        [SerializeField]
        private Button btn_Delete;

        [SerializeField]
        private Button btn_leftRotate;

        [SerializeField]
        private Button btn_rightRotate;

        [SerializeField]
        private Button btn_accept;

        private PlacementPreview placementPreview;

        private Building building;

        private int angle = 0;


        private void Start()
        {
            btn_Delete.onClick.AddListener(OnDeleted);
            btn_leftRotate.onClick.AddListener(OnLeftRotated);
            btn_rightRotate.onClick.AddListener(OnRightRotated);
            btn_accept.onClick.AddListener(OnAccepted);
            gridObjectSelector.ObjectSelect += OnObjectSelected;
        }

        private void OnDestroy()
        {
            btn_Delete.onClick.RemoveListener(OnDeleted);
            btn_leftRotate.onClick.RemoveListener(OnLeftRotated);
            btn_rightRotate.onClick.RemoveListener(OnRightRotated);
            btn_accept.onClick.RemoveListener(OnAccepted);
            gridObjectSelector.ObjectSelect -= OnObjectSelected;
        }

        private void OnDeleted()
        {
            if (building != null)
            {
                if (GameStateManager.CurrentGameState == GameState.Building)
                {
                    building = null;
                    return;
                }
                buildingManager.Remove(building);
                building = null;
            }
        }

        private void OnLeftRotated()
        {
            angle -= 90;
            HandlePlacementPreviewRotation(angle);
        }

        private void OnRightRotated()
        {
            angle += 90;
            HandlePlacementPreviewRotation(angle);
        }

        private void OnAccepted()
        {
            if (placementPreview != null)
            {
                bool isBuilt = placementPreview.Build();

                if (isBuilt)
                {
                    placementPreview = null;
                }
            }
        }

        private void OnObjectSelected(Building building)
        {
            this.building = building;
        }

        public void SetPlacementPreviewInstanec(PlacementPreview placementPreview)
        {
            this.placementPreview = placementPreview;
        }

        private void HandlePlacementPreviewRotation(int angle)
        {
            if (placementPreview != null)
            {
                placementPreview.transform.rotation = Quaternion.Euler(0, placementPreview.transform.rotation.y + angle, 0);
            }
            else if (building != null)
            {
                building.transform.rotation = Quaternion.Euler(0, building.transform.rotation.y + angle, 0);
            }
        }

    }
}
