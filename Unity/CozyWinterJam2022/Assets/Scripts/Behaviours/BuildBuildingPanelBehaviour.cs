namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildBuildingPanel")]

    public class BuildBuildingPanelBehaviour : MonoBehaviour
    {
        #region Members

        protected BuildingPlacerBehaviour BuildingPlacer { get; set; }       

        #endregion

        #region Events

        #endregion

        #region Methods

        public void SelectBuildingType()
        {            
            transform.localScale = new Vector3(0, 0, 0);
        }

        protected void Awake()
        {
            BuildingPlacer = FindObjectOfType<BuildingPlacerBehaviour>(true);

            transform.localScale = new Vector3(0, 0, 0);
        }

        protected void Update()
        {
            if (Input
                .GetMouseButtonDown(1))
            {
                var rectTransform = GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(
                    Input.mousePosition.x - rectTransform.sizeDelta.x / 2,
                    Input.mousePosition.y - rectTransform.sizeDelta.y / 2);

                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
        #endregion
    }
}