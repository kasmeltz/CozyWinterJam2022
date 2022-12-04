namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/BuildBuilding")]

    public class BuildBuildingBehaviour : MonoBehaviour, IPointerEnterHandler
    {
        #region Members

        public BuildingType BuildingType;
        public BuildingPlacerBehaviour BuildingPlacer;
        
        protected BuildBuildingPanelBehaviour BuildBuildingPanel { get; set; }

        #endregion

        #region Methods

        public void ClickBuildBuilding()
        {
            BuildingPlacer.TypeToBuild = BuildingType;

            BuildingPlacer
                .gameObject
                .SetActive(true);
        }
     
        protected void Awake()
        {
            BuildBuildingPanel = FindObjectOfType<BuildBuildingPanelBehaviour>(true);
            
            //GetComponent<Image>()
               //.alphaHitTestMinimumThreshold = 0.1f;
        }

        #endregion

        #region IPointerEnterHandler

        public void OnPointerEnter(PointerEventData eventData)
        {
            BuildBuildingPanel
                .ShowCost(BuildingType);
        }


        #endregion
    }
}