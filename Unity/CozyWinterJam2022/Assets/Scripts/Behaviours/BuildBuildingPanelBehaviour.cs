namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/BuildBuildingPanel")]

    public class BuildBuildingPanelBehaviour : MonoBehaviour
    {
        #region Members

        protected BuildingPlacerBehaviour BuildingPlacer { get; set; }       

        protected GameworldBehaviour Gameworld { get; set; }

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
            Gameworld = FindObjectOfType<GameworldBehaviour>(true);

            transform.localScale = new Vector3(0, 0, 0);
        }

        protected void Update()
        {
            if (Input
                .GetMouseButtonDown(1))
            {
                if (transform.localScale.x == 0)
                {
                    var buildBuildings = GetComponentsInChildren<BuildBuildingBehaviour>();
                    foreach (var buildBuilding in buildBuildings)
                    {
                        var isAvailable = Gameworld
                            .IsBuildingAvailable(buildBuilding.BuildingType);

                        var button = buildBuilding
                            .GetComponent<Button>();

                        button.interactable = isAvailable;
                    }

                    var rectTransform = GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2(
                        Input.mousePosition.x - rectTransform.sizeDelta.x / 2,
                        Input.mousePosition.y - rectTransform.sizeDelta.y / 2);

                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(0, 0, 0);
                }
            }
        }
        
        #endregion
    }
}