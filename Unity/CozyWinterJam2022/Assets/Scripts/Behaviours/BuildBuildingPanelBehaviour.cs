namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/BuildBuildingPanel")]

    public class BuildBuildingPanelBehaviour : MonoBehaviour
    {
        #region Members

        public TMP_Text[] Texts;
        public Image[] Images;

        protected BuildingPlacerBehaviour BuildingPlacer { get; set; }       

        protected GameworldBehaviour Gameworld { get; set; }

        #endregion

        #region Events

        #endregion

        #region Methods

        public void ShowCost(BuildingType buildingType)
        {
            var categories = Gameworld.BuildingResourceCostCategories[buildingType];
            var amounts = Gameworld.BuildingResourceCostAmounts[buildingType];

            for(int i =0;i < Images.Length;i++)
            {
                if (i >= categories.Count)
                {
                    Images[i]
                        .gameObject
                        .SetActive(false);

                    Texts[i]
                        .gameObject
                        .SetActive(false);

                    continue;
                }

                var sprites = Resources
                    .LoadAll<Sprite>("Images/resource_icons");

                Images[i]
                    .gameObject
                    .SetActive(true);

                Texts[i]
                    .gameObject
                    .SetActive(true);
          
                var category = categories[i];
                var amount = amounts[i];

                int imageIndex = 0;
                switch(category)
                {
                    case ProduceableResourceCategory.Food:
                        imageIndex = 0;
                        break;

                    case ProduceableResourceCategory.Wood:
                        imageIndex = 1;
                        break;

                    case ProduceableResourceCategory.Coal:
                        imageIndex = 2;
                        break;

                    case ProduceableResourceCategory.Gingerbread:
                        imageIndex = 3;
                        break;

                    case ProduceableResourceCategory.Cookies:
                        imageIndex = 4;
                        break;                                        

                    case ProduceableResourceCategory.Present1:
                        imageIndex = 5;
                        break;

                    case ProduceableResourceCategory.Present2:
                        imageIndex = 6;
                        break;
                }

                Texts[i].text = $"{amount}";
                Images[i].sprite = sprites[imageIndex];
            }
        }

        public void SelectBuildingType()
        {            
            transform.localScale = new Vector3(0, 0, 0);
        }

        protected void Awake()
        {
            BuildingPlacer = FindObjectOfType<BuildingPlacerBehaviour>(true);
            Gameworld = FindObjectOfType<GameworldBehaviour>(true);

            transform.localScale = new Vector3(0, 0, 0);
            
            foreach(var image in Images)
            {
                image
                    .gameObject
                    .SetActive(false);
            }

            foreach (var text in Texts)
            {
                text
                    .gameObject
                    .SetActive(false);
            }
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