namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("CWJ2022/ToDoList")]

    public class ToDoListBehaviour : MonoBehaviour
    {
        #region Members

        public TMP_Text[] Texts;

        #endregion

        #region Event Handlers


        #endregion

        #region Methods

        public void SetGoals(List<Tuple<ProduceableResourceCategory, float>> goals)
        {
            for(int i = 0;i < Texts.Length;i++)
            {
                var textObject = Texts[i];

                if (i >= goals.Count)
                {
                    textObject
                        .gameObject
                        .SetActive(false);

                    continue;
                }

                var goal = goals[i];

                textObject
                    .gameObject
                    .SetActive(true);

                string goalName = "";

                switch (goal.Item1)
                {
                    case ProduceableResourceCategory.Present1:
                        goalName = "Simple Presents";
                        break;
                    case ProduceableResourceCategory.Present2:
                        goalName = "Advanced Presents";
                        break;
                    case ProduceableResourceCategory.Present3:
                        goalName = "Technlogical Presents";
                        break;

                }

                textObject
                    .text = $"Create {goal.Item2} {goalName}";
            }
        }
        
        #endregion
    }
}