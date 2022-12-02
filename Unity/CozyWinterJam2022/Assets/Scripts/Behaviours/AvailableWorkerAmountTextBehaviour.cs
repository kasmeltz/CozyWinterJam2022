namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using TMPro;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/AvailableWorkerAmountText")]

    public class AvailableWorkerAmountTextBehaviour : MonoBehaviour
    {
        #region Members

        public TMP_Text TextObject;
        public WorkerCategory WorkerCategory;

        protected GameworldBehaviour Gameworld { get; set;}

        #endregion

        #region Methods

        protected void Update()
        {
            int index = (int)WorkerCategory;

            var amount = Gameworld.AvailableWorkers[index];

            TextObject.text = amount
                .ToString();
        }

        protected void Awake()
        {
            Gameworld = FindObjectOfType<GameworldBehaviour>();
        }

        #endregion
    }
}