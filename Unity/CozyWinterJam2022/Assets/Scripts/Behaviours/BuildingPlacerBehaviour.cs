namespace HNS.CozyWinterJam2022.Behaviours
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildingPlacer")]

    public class BuildingPlacerBehaviour : MonoBehaviour
    {        
        protected Plane GroundPlane { get; set; }

        protected Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        protected void Awake()
        {
            Buildings = new Dictionary<Tuple<float, float>, BuildingBehaviour>();
            GroundPlane = new Plane(Vector3.up, 0);
        }

        protected void BuildBuilding(float x, float z)
        {
            var key = new Tuple<float, float>(x, z);

            if (Buildings
                .ContainsKey(key))
            {
                return;
            }

            var prefab = Resources.Load<BuildingBehaviour>("Prefabs/Building");

            var building = Instantiate(prefab);

            building.transform.position = transform.position;

            building
                .PositionProgressBar();

            gameObject
                .SetActive(false);

            Buildings[key] = building;
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

            transform.position = new Vector3(x, 0, z);

            if (Input
                .GetMouseButtonDown(0))
            {
                BuildBuilding(x, z);
            }
        }
    }
}