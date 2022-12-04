namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    [AddComponentMenu("CWJ2022/CameraMove")]

    public class CameraMoveBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Members

        public Vector3 MoveDirection;
        public Rect CameraBounds;

        protected bool IsMoving { get; set; }
        
        #endregion

        #region Events

        #endregion

        #region Methods       

        public void MoveCamera()
        {
            var position = Camera.main.transform.position;
            position += MoveDirection * Time.deltaTime;

            if (position.x < CameraBounds.x)
            {
                position.x = CameraBounds.x;
            }

            if (position.x > CameraBounds.xMax)
            {
                position.x = CameraBounds.xMax;
            }

            if (position.z < CameraBounds.y)
            {
                position.z = CameraBounds.y;
            }

            if (position.z > CameraBounds.yMax)
            {
                position.z = CameraBounds.yMax;
            }

            Camera.main.transform.position = position;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsMoving = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsMoving = false;
        }

        protected void Update()
        {
            if (IsMoving)
            {
                MoveCamera();
            }
        }

        #endregion
    }
}