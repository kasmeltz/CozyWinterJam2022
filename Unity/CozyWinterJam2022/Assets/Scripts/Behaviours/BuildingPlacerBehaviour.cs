namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("CWJ2022/BuildingPlacer")]

    public class BuildingPlacerBehaviour : MonoBehaviour
    {        
        protected void Update()
        {
            var mousePos = Input.mousePosition;

            var mx = mousePos.x;
            var my = mousePos.y;

            var xGrid = Screen.width / 50.0f;
            var x = -25 + (mx / xGrid);
            x = Mathf
                .Round(x);

            /*
            var zGrid = Screen.width / 20.0f;
            var z = -10 + (my / zGrid);
            z = Mathf
                .Round(z);
            */

            /*
            var x = Mathf
                .Round(worldPosition.x);

            var y = Mathf
                .Round(worldPosition.y);

            var z = Mathf
                .Round(worldPosition.z);

            
            */

            var z = 0;

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