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

        private void Start()
        {
            RegisterUIEvents();
            GridManager.Instance.ObjectSelected += OnObjectSelected;
        }

        private void OnDestroy()
        {
            UnregisterUIEvents();
            GridManager.Instance.ObjectSelected -= OnObjectSelected;
        }

        private void RegisterUIEvents()
        {
            btn_Delete.onClick.AddListener(() => GridManager.Instance.DeleteBuilding());
            btn_leftRotate.onClick.AddListener(() => GridManager.Instance.RotateLeftBuilding());
            btn_rightRotate.onClick.AddListener(() => GridManager.Instance.RotateRightBuilding());
            btn_accept.onClick.AddListener(() => GridManager.Instance.AcceptPlacement());
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
            GridManager.Instance.SetBuildingInstance(building);
        }

        public void SetPlacementPreviewInstance(PlacementPreview placementPreview)
        {
            GridManager.Instance.SetPlacementPreviewInstance(placementPreview);
        }
    }
}
