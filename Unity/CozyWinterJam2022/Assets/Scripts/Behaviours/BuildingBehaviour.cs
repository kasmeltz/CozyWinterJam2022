namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/Building")]

    public class BuildingBehaviour : MonoBehaviour
    {
        #region Members

        public Image ProgressBarBackground;
        public Image ProgressBarForeground;
        public Vector2 ProgressBarOffset;
        public float BuildSeconds;
        public float BuildSpeed;
        public BuildingType BuildingType;

        public ProduceableResourceCategory[] ResourcesProducedCategories;
        public float[] ResourcesProducedAmounts;

        public WorkerCategory[] WorkersProducedCategories;
        public int[] WorkersProducedAmounts;

        public float BuildProgress { get; set; }

        public bool IsBuilt { get; set; }

        public int[] WorkersPresent { get; set; }
        
        #endregion

        #region Events

        public event EventHandler BuildComplete;
        protected void OnBuildComplete()
        {
            BuildComplete?
                .Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        protected void OnMouseDown()
        {
            var buildingWorkerPabel = FindObjectOfType<BuildingWorkerPanelBehaviour>(true);

            buildingWorkerPabel
                .SetActiveBuilding(this);
        }

        protected void Awake()
        {
            var workers = Enum
               .GetValues(typeof(WorkerCategory));

            WorkersPresent = new int[workers.Length];

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

                OnBuildComplete();
            }

            ProgressBarForeground.fillAmount = (BuildProgress / BuildSeconds);
        }

        protected void Update()
        {
            PositionProgressBar();
            UpdateProgessBar();
        }

        #endregion
    }
}