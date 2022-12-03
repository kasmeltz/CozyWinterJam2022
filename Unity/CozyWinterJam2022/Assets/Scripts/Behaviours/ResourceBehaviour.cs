namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/Resource")]

    public class ResourceBehaviour : MonoBehaviour
    {
        #region Members

        public ProduceableResourceCategory ResourceCategory;
        public float StartingAmount;
        public float PerTickUsedAmount;

        public float AmounLeft { get; set; }

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}