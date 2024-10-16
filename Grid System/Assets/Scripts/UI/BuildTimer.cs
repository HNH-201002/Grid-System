using System;
using UnityEngine;
using UnityEngine.UI;

namespace GridSystem.UIManager
{
    public class BuildTimer : MonoBehaviour
    {
        [SerializeField]
        private Slider timerSlider;

        [SerializeField]
        private float buildTime = 1f;

        private float timeRemaining;

        public event Action BuildCompleted;

        private bool isBuilding = false;

        private void Start()
        {
            timerSlider.gameObject.SetActive(false);
            ResetTimer();
        }

        public void StartBuilding()
        {
            if (!isBuilding)
            {
                ResetTimer();
                timerSlider.maxValue = buildTime;
                timeRemaining = 0;
                timerSlider.value = 0;
                timerSlider.gameObject.SetActive(true);
                isBuilding = true;
            }
        }

        public void ResetTimer()
        {
            isBuilding = false;
            timeRemaining = 0;
            timerSlider.value = 0;
            timerSlider.gameObject.SetActive(false);
        }

        public void UpdateTimer()
        {
            if (isBuilding && timeRemaining < buildTime)
            {
                timeRemaining += Time.deltaTime;
                timerSlider.value = timeRemaining;

                if (timeRemaining >= buildTime)
                {
                    BuildCompleted?.Invoke();
                }
            }
        }
    }
}
