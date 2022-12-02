namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildingPlacer")]

    public class BuildingPlacerBehaviour : MonoBehaviour
    {        
        protected void Update()
        {
            var mousePos = Input.mousePosition;

            /*
            float dz = transform.position.z - Camera.main.transform.position.z;
            float dx = transform.position.x - Camera.main.transform.position.x;
            float d = Mathf
                .Sqrt((dx * dx) + (dz * dz));
            */

            var d = 10;

            var worldPosition = Camera
                .main
                .ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, d));

            var x = Mathf
                .Round(worldPosition.x);

            var z = Mathf
                .Round(worldPosition.z);

            transform.position = new Vector3(x, 0, z);

            if (Input
                .GetMouseButtonDown(0))
            {
                var prefab = Resources.Load<BuildingBehaviour>("Prefabs/Building");

                var building = Instantiate(prefab);

                building.transform.position = transform.position;
                
                building
                    .PositionProgressBar();

                gameObject
                    .SetActive(false);
            }
        }
    }
}