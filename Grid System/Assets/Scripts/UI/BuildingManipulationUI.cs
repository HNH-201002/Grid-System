using GridSystem.Core;
using GridSystem.Visualization;
using UnityEngine;
using UnityEngine.UI;

namespace GridSystem.UIManager
{
    /// <summary>
    /// Provides UI controls for manipulating building objects, including deletion, rotation, and confirmation.
    /// </summary>
    public class BuildingManipulationUI : MonoBehaviour
    {
        [SerializeField]
        private Button btn_Delete;

        [SerializeField]
        private Button btn_leftRotate;

        [SerializeField]
        private Button btn_rightRotate;

        [SerializeField]
        private Button btn_accept;

        [SerializeField]
        private GridManager gridManager;

        private void Start()
        {
            RegisterUIEvents();
            gridManager.ObjectSelected += OnObjectSelected;
        }

        private void OnDestroy()
        {
            UnregisterUIEvents();
            gridManager.ObjectSelected -= OnObjectSelected;
        }

        private void RegisterUIEvents()
        {
            btn_Delete.onClick.AddListener(() => gridManager.DeleteBuilding());
            btn_leftRotate.onClick.AddListener(() => gridManager.RotateLeftBuilding());
            btn_rightRotate.onClick.AddListener(() => gridManager.RotateRightBuilding());
            btn_accept.onClick.AddListener(() => gridManager.AcceptPlacement());
        }

        private void UnregisterUIEvents()
        {
            btn_Delete.onClick.RemoveAllListeners();
            btn_leftRotate.onClick.RemoveAllListeners();
            btn_rightRotate.onClick.RemoveAllListeners();
            btn_accept.onClick.RemoveAllListeners();
        }

        private void OnObjectSelected(Building building)
        {
            gridManager.SetBuildingInstance(building);
        }

        public void SetPlacementPreviewInstance(PlacementPreview placementPreview)
        {
            gridManager.SetPlacementPreviewInstance(placementPreview);
        }
    }
}
