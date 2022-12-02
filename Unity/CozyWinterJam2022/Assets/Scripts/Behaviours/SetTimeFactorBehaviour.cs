namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;

    [AddComponentMenu("CWJ2022/SetTimeFactor")]

    public class SetTimeFactorBehaviour : MonoBehaviour
    {        
        #region Methods

        public void ClickSetTimeFactor(float timeScale)
        {
            Time.timeScale = timeScale;
        }

        #endregion
    }
}