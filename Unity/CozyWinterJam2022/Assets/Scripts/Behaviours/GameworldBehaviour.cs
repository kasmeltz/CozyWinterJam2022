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

            if (Buildings.Count > 1)
            {
                var roads = FindObjectOfType<RoadsBehaviour>();

                var firstVertex = new Vector3(building.transform.position.x, -0.5f, building.transform.position.z);
                var secondVertex = new Vector3(building.transform.position.x, -0.5f, building.transform.position.z);

                float minDistance = float.MaxValue;
                foreach (var otherBuilding in Buildings.Keys)
                {
                    var dx = otherBuilding.Item1 - firstVertex.x;
                    var dy = otherBuilding.Item2 - firstVertex.z;

                    var d = (dx * dx) + (dy * dy);
                    if (d > 0 && d < minDistance)
                    {
                        minDistance = d;
                        secondVertex.x = otherBuilding.Item1;
                        secondVertex.z = otherBuilding.Item2;
                    }
                }

                roads
                    .AddRoads(firstVertex, secondVertex);
            }
        }

        #endregion

        #region Methods

        public bool BuildingExists(float x, float z)
        {
            var key = new Tuple<float, float>(x, z);

            return Buildings
                .ContainsKey(key);
        }

        public void AddBuilding(float x, float z, BuildingBehaviour building)
        {
            var key = new Tuple<float, float>(x, z);
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