namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using TMPro;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/ResourceAmountText")]

    public class ResourceAmountTextBehaviour : MonoBehaviour
    {
        #region Members

        public TMP_Text TextObject;
        public ProduceableResourceCategory ProduceableResourceCategory;

        protected GameworldBehaviour Gameworld { get; set;}

        #endregion

        #region Methods

        protected void Update()
        {
            int index = (int)ProduceableResourceCategory;

            var amount = Gameworld.Inventory[index];

            TextObject.text = Mathf
                .Round(amount)
                .ToString();
        }

        protected void Awake()
        {
            Gameworld = FindObjectOfType<GameworldBehaviour>();
        }

        #endregion
    }
}