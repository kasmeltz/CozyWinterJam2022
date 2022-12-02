namespace HNS.CozyWinterJam2022.Behaviours
{
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/YearTimeProgressBar")]

    public class YearTimeProgressBarBehaviour : MonoBehaviour
    {
        #region Members

        public Image Foreground;

        protected GameworldBehaviour Gameworld { get; set; }

        #endregion

        #region Event Handlers

        #endregion

        #region Methods
       
        protected void Update()
        {
            float ratio = Gameworld.YearTimeLeft / Gameworld.YearTimeToStart;
            Foreground.fillAmount = ratio;
        }

        protected void Awake()
        {
            Gameworld = FindObjectOfType<GameworldBehaviour>();            
        }
       
        #endregion
    }
}