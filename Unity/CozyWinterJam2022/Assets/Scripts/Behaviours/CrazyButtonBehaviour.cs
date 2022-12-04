namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/CrazyButton")]

    public class CrazyButtonBehaviour : MonoBehaviour
    {
        #region Members
        
        #endregion

        #region Events

        #endregion

        #region Methods       

        protected void Start()
        {
            GetComponent<Image>()
                .alphaHitTestMinimumThreshold = 0.1f;
        }

        #endregion
    }
}