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

        public float BuildProgress;

        protected void Awake()
        {
            BuildProgress = 0;
        }

        public void PositionProgressBar()
        {
            var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            ProgressBarBackground.rectTransform.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y) + ProgressBarOffset;

            ProgressBarForeground.fillAmount = BuildProgress;
        }

        protected void Update()
        {
            PositionProgressBar();

            BuildProgress += Time.deltaTime / 2;

            if (BuildProgress > 1)
            {
                BuildProgress = 1;
            }

            ProgressBarForeground.fillAmount = BuildProgress;
        }
    }
}