using GridSystem.Data;
using UnityEngine;
using UnityEngine.UI;
using GridSystem.Core;
using GridSystem.Utilities;

namespace GridSystem.UIManager
{
    [RequireComponent(typeof(Button))]
    public class ButtonBuildingPreview : MonoBehaviour
    {
        [SerializeField]
        private BuildingPrefabData buildingPrefabData;

        private Button btn_previewToggle;

        private Camera mainCamera;
        private PreviewController previewController;

        private void Start()
        {
            btn_previewToggle = GetComponent<Button>();
            mainCamera = Camera.main;

            if (buildingPrefabData == null || buildingPrefabData.Prefab == null)
            {
                Debug.LogError("Building prefab data or prefab is missing.");
                btn_previewToggle.interactable = false;
                return;
            }

            previewController = GridManager.Instance.InitializePreviewController(buildingPrefabData);
            btn_previewToggle.onClick.AddListener(TogglePreview);
        }

        private void OnDestroy()
        {
            btn_previewToggle.onClick.RemoveListener(TogglePreview);
        }

        private void TogglePreview()
        {
            previewController.TogglePreviewState();
        }
    }
}
