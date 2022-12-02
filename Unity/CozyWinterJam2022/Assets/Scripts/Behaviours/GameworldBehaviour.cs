namespace HNS.CozyWinterJam2022.Behaviours
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("CWJ2022/Gameworld")]

    public class GameworldBehaviour : MonoBehaviour
    {
        public Dictionary<Tuple<float, float>, BuildingBehaviour> Buildings { get; set; }

        protected void Awake()
        {
            Buildings = new Dictionary<Tuple<float, float>, BuildingBehaviour>();
        }
    }
}