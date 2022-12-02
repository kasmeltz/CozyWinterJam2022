namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildBuilding")]

    public class BuildBuildingBehaviour : MonoBehaviour
    {
        #region Members

        public BuildingPlacerBehaviour BuildingPlacer;

        #endregion

        #region Methods

        public void ClickBuildBuilding()
        {
            BuildingPlacer
                .gameObject
                .SetActive(true);
        }

        #endregion
    }
}