namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/Gameworld")]

    public class GameworldBehaviour : MonoBehaviour
    {
        #region Members

        public Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        public float[] Production { get; set; }

        public float[] Inventory { get; set; }

        #endregion

        #region Event Handlers

        private void Building_BuildComplete(object sender, EventArgs e)
        {
            var building = (BuildingBehaviour)sender;

            for (int i = 0; i < building.ResourcesProducedCategories.Length; i++)
            {
                var category = building.ResourcesProducedCategories[i];
                var amount = building.ResourcesProducedAmounts[i];

                var categoryIndex = (int)category;
                Production[categoryIndex] += amount;
            }
        }

        #endregion

        #region Methods

        public bool BuildingExists(Tuple<float, float> key)
        {
            return Buildings
                .ContainsKey(key);
        }

        public void AddBuilding(Tuple<float, float> key, BuildingBehaviour building)
        {
            Buildings[key] = building;
            building.BuildComplete += Building_BuildComplete;
        }

        protected void Update()
        {
            for (int i = 0; i < Production.Length; i++)
            {
                Inventory[i] += Production[i] * Time.deltaTime;
            }
        }

        protected void Awake()
        {
            Buildings = new Dictionary<Tuple<float, float>, BuildingBehaviour>();

            var resources = Enum
                .GetValues(typeof(ProduceableResourceCategory));

            Production = new float[resources.Length];
            Inventory = new float[resources.Length];
        }
       
        #endregion
    }
}