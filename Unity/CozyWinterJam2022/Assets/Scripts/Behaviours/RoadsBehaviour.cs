namespace HNS.CozyWinterJam2022.Behaviours
{
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/Roads")]

    public class RoadsBehaviour : MonoBehaviour
    {
        #region Members

        public LineRenderer LineRendererPrefab;

        protected List<LineRenderer> RoadLines { get; set; }

        #endregion

        #region Methods

        public void AddRoads(Vector3 position1, Vector3 position2)
        {
            var lineRenderer = Instantiate(LineRendererPrefab);

            lineRenderer
                .positionCount = 2;

            lineRenderer
                .SetPosition(0, position1);

            lineRenderer
                .SetPosition(1, position2);

            RoadLines
                .Add(lineRenderer);
        }

        public void ClearAllRoads()
        {
            var toDestroy = new List<LineRenderer>();
            toDestroy
                .AddRange(RoadLines);

            foreach(var line in toDestroy)
            {
                Destroy(line.gameObject);
            }

            RoadLines
                .Clear();
        }

        protected void Awake()
        {
            RoadLines = new List<LineRenderer>();
        }

        #endregion
    }
}