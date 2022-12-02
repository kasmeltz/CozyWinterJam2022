namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildBuilding")]

    public class BuildBuildingBehaviour : MonoBehaviour
    {
        public BuildingPlacerBehaviour BuildingPlacer;

        public void ClickBuildBuilding()
        {
            BuildingPlacer
                .gameObject
                .SetActive(true);
        }

    }
}