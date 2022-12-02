namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/Building")]

    public class BuildingBehaviour : MonoBehaviour
    {
        public Image ProgressBarBackground;
        public Image ProgressBarForeground;
        public Vector2 ProgressBarOffset;
        public float BuildSeconds;
        public float BuildSpeed;

        public float BuildProgress { get; set; }

        public bool IsBuilt { get; set; }

        protected void Awake()
        {
            BuildProgress = 0;
            IsBuilt = false;
            ProgressBarBackground
                .gameObject
                .SetActive(true);
        }

        public void PositionProgressBar()
        {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            ProgressBarBackground.rectTransform.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y) + ProgressBarOffset;
            ProgressBarForeground.fillAmount = BuildProgress;            
        }

        public void UpdateProgessBar()
        {
            if (IsBuilt)
            {
                return;
            }

            BuildProgress += Time.deltaTime * BuildSpeed;

            if (BuildProgress >= BuildSeconds)
            {
                BuildProgress = BuildSeconds;
                IsBuilt = true;
                ProgressBarBackground
                    .gameObject
                    .SetActive(false);
            }

            ProgressBarForeground.fillAmount = (BuildProgress / BuildSeconds);
        }

        protected void Update()
        {
            PositionProgressBar();
            UpdateProgessBar();
        }
    }
}