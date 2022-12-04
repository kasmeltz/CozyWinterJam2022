namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/BuildingPlacer")]

    public class BuildingPlacerBehaviour : MonoBehaviour
    {
        #region Members

        public Image UIPanel;
        public TMP_Text AmountText;
        public Vector2 UIPanelOffset;

        public BuildingType TypeToBuild { get; set; }

        protected Plane GroundPlane { get; set; }

        protected GameworldBehaviour Gameworld { get; set; }

        #endregion

        #region Methods

        protected void Awake()
        {
            Gameworld = FindObjectOfType<GameworldBehaviour>(true);
            GroundPlane = new Plane(Vector3.up, 0);
        }

        protected void BuildBuilding(float x, float z)
        {
            Gameworld
                .BuildBuilding(x, z, TypeToBuild);

            gameObject
                .SetActive(false);
        }

        protected void UpdateUI()
        {          
            if (TypeToBuild == BuildingType.Lumbercamp ||
                TypeToBuild == BuildingType.Farm ||
                TypeToBuild == BuildingType.HuntingLodge ||
                TypeToBuild == BuildingType.GingerbreadQuarry ||
                TypeToBuild == BuildingType.CoalMine)
            {
                var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                UIPanel.rectTransform.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y) + UIPanelOffset;

                int categoryIndex = 0;
                switch (TypeToBuild)
                {
                    case BuildingType.Lumbercamp:
                        categoryIndex = (int)ProduceableResourceCategory.Wood;
                        break;

                    case BuildingType.Farm:
                        categoryIndex = -1;
                        break;

                    case BuildingType.HuntingLodge:
                        categoryIndex = (int)ProduceableResourceCategory.Wood;
                        break;

                    case BuildingType.GingerbreadQuarry:
                        categoryIndex = (int)ProduceableResourceCategory.Gingerbread;
                        break;

                    case BuildingType.CoalMine:
                        categoryIndex = (int)ProduceableResourceCategory.Coal;
                        break;
                }

                var resourcesFound = Gameworld
                    .CountResourcesForBuilding(transform.position, categoryIndex);

                AmountText.text = resourcesFound
                    .ToString();

                UIPanel
                    .gameObject
                    .SetActive(true);

            }
            else
            {
                UIPanel
                    .gameObject
                    .SetActive(false);
            }
        }

        protected void Update()
        {            
            var worldPosition = transform.position;
            
            float distance;
            Ray ray = Camera
                .main
                .ScreenPointToRay(Input.mousePosition);

            if (GroundPlane
                .Raycast(ray, out distance))
            {
                worldPosition = ray
                    .GetPoint(distance);
            }

            var x = Mathf
                .Round(worldPosition.x);

            var z = Mathf
                .Round(worldPosition.z);

            if (x < -25)
            {
                x = -25;
            }
            else if (x > 25)
            {
                x = 25;
            }

            if ( z < -25)
            {
                z = -25;
            }            
            else if (z > 25)
            {
                z = 25;
            }

            transform.position = new Vector3(x, 0, z);
            
            UpdateUI();
            
            if (Input
                .GetMouseButtonDown(0))
            {
                BuildBuilding(x, z);
            }

            if (Input
                .GetMouseButtonDown(1))
            {
                gameObject
                    .SetActive(false);
            }
        }

        #endregion
    }
}