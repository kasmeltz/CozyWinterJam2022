namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildingPlacer")]

    public class BuildingPlacerBehaviour : MonoBehaviour
    {
        #region Members

        public BuildingType TypeToBuild { get; set; }

        protected Plane GroundPlane { get; set; }

        protected GameworldBehaviour Gameworld { get; set; }

        #endregion

        #region Methods

        protected void Awake()
        {
            Gameworld = FindObjectOfType<GameworldBehaviour>();
            GroundPlane = new Plane(Vector3.up, 0);
        }

        protected void BuildBuilding(float x, float z)
        {
            if (Gameworld
                .BuildingExists(x, z))
            {
                return;
            }

            string prefabeName = "";
            switch(TypeToBuild)
            {
                case BuildingType.SantasWorkshop:
                    prefabeName = "SantasWorkshop";
                    break;

                case BuildingType.House:
                    prefabeName = "House";
                    break;
            }

            var prefab = Resources
                .Load<BuildingBehaviour>($"Prefabs/{prefabeName}");

            var building = Instantiate(prefab);

            building.transform.position = transform.position;

            building
                .PositionProgressBar();

            gameObject
                .SetActive(false);

            Gameworld
                .AddBuilding(x, z, building);
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