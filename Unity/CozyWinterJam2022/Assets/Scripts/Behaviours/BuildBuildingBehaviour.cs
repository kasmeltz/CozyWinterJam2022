namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildBuilding")]

    public class BuildBuildingBehaviour : MonoBehaviour
    {
        #region Members

        public BuildingType BuildingType;
        public BuildingPlacerBehaviour BuildingPlacer;

        #endregion

        #region Methods

        public void ClickBuildBuilding()
        {
            BuildingPlacer.TypeToBuild = BuildingType;

            BuildingPlacer
                .gameObject
                .SetActive(true);
        }

        #endregion
    }
}