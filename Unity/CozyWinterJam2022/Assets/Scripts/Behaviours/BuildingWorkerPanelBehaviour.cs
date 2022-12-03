namespace HNS.CozyWinterJam2022.Behaviours
{
    using TMPro;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildingWorkerPanel")]
    public class BuildingWorkerPanelBehaviour : MonoBehaviour
    {
        #region Members

        public TMP_Text[] WorkerCountTexts;

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
            
            var available = Gameworld.AvailableWorkers[workerType];

            if (available <= 0)
            {
                return;
            }

            Gameworld.AvailableWorkers[workerType]--;
            SelectedBuilding.WorkersPresent[workerType]++;
        }

        public void RemoveWorker(int workerType)
        {
            if (SelectedBuilding == null)
            {
                return;
            }

            var available = SelectedBuilding.WorkersPresent[workerType];
            if (available <= 0)
            {
                return;
            }

            Gameworld.AvailableWorkers[workerType]++;
            SelectedBuilding.WorkersPresent[workerType]--;
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

            for (int i = 0; i < SelectedBuilding.WorkersPresent.Length; i++)
            {
                if (i >= WorkerCountTexts.Length)
                {
                    break;
                }

                var workersPresent = SelectedBuilding.WorkersPresent[i];

                var workerText = WorkerCountTexts[i];
                workerText.text = workersPresent
                    .ToString();
            }
        }

        #endregion
    }
}