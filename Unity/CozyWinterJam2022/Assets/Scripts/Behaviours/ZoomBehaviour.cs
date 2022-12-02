namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("CWJ2022/Zoom")]

    public class ZoomBehaviour : MonoBehaviour
    {
        #region Members

        public Camera Camera;
        public float ZoomScale;
        public Vector2 MinMaxZoom;

        #endregion

        #region Events

        #endregion

        #region Methods

        protected void Awake()
        {            
        }
       
        protected void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                var camera = Camera;

                camera.orthographicSize -= (Input.mouseScrollDelta.y * ZoomScale);

                if (camera.orthographicSize < MinMaxZoom.x)
                {
                    camera.orthographicSize = MinMaxZoom.x;
                }

                if (camera.orthographicSize > MinMaxZoom.y)
                {
                    camera.orthographicSize = MinMaxZoom.y;
                }
            }
        }

        #endregion
    }
}