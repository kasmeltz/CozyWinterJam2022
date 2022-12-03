namespace HNS.CozyWinterJam2022.Behaviours
{
    using TMPro;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildingWorkerPanel")]
    public class BuildingWorkerPanelBehaviour : MonoBehaviour
    {
        #region Members

        public TMP_Text WorkerNameText;
        public TMP_Text WorkerCountText;

        public GameworldBehaviour Gameworld { get; set; }

        public BuildingBehaviour SelectedBuilding { get; set; }        

        #endregion

        #region Events

        #endregion

        #region Methods

        public void SetActiveBuilding(BuildingBehaviour building)
        {
            SelectedBuilding = building;

            gameObject
                .SetActive(true);
        }

        public void AddWorker(int workerType)
        {
            if (SelectedBuilding == null)
            {
                return;
            }
            
            if ((int)SelectedBuilding.WorkerCategory != workerType)
            {
                return;
            }

            if (SelectedBuilding.WorkersPresent >= SelectedBuilding.MaxWorkers)
            {
                return;
            }

            var available = Gameworld.AvailableWorkers[workerType];

            if (available <= 0)
            {
                return;
            }
            
            

            Gameworld.AvailableWorkers[workerType]--;
            SelectedBuilding.WorkersPresent++;
        }

        public void RemoveWorker(int workerType)
        {
            if (SelectedBuilding == null)
            {
                return;
            }

            if ((int)SelectedBuilding.WorkerCategory != workerType)
            {
                return;
            }

            var available = SelectedBuilding.WorkersPresent;
            if (available <= 0)
            {
                return;
            }

            Gameworld.AvailableWorkers[workerType]++;
            SelectedBuilding.WorkersPresent--;
        }

        protected void Awake()
        {
            Gameworld = FindObjectOfType<GameworldBehaviour>(true);
        }

        protected void Update()
        {
            if (SelectedBuilding == null)
            {
                return;
            }

            var workersPresent = SelectedBuilding.WorkersPresent;

            string workerName = "";

            switch(SelectedBuilding.WorkerCategory)
            {
                case Models.WorkerCategory.WorkerType1:
                    workerName = "Worker";
                    break;

                case Models.WorkerCategory.WorkerType2:
                    workerName = "Artisan";
                    break;

                case Models.WorkerCategory.WorkerType3:
                    workerName = "Artificer";
                    break;
            }
            WorkerNameText.text = workerName;
            WorkerCountText.text = $"{workersPresent} / {SelectedBuilding.MaxWorkers}";
        }

        #endregion
    }
}