namespace HNS.CozyWinterJam2022.Behaviours
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/Gameworld")]

    public class GameworldBehaviour : MonoBehaviour
    {
        #region Members

        public Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        #endregion

        #region Event Handlers

        private void Building_BuildComplete(object sender, EventArgs e)
        {
            // do something when building is built
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

        protected void Awake()
        {
            Buildings = new Dictionary<Tuple<float, float>, BuildingBehaviour>();
        }
       
        #endregion
    }
}