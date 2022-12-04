namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DialogueBehaviour : MonoBehaviour
    {
        public TMP_Text ButtonText;
        public Image SpeakerLeftImage;
        public Image SpeakerRightImage;
        public TextMeshProUGUI text;        
        int LinesPlace = 0;

        public List<DialogLine[]> Lines { get; set; }

        protected GameworldBehaviour Gameworld { get; set; }

        protected int Year { get; set; }

        private void Awake()
        {
            Lines = new List<DialogLine[]>();

            Gameworld = FindObjectOfType<GameworldBehaviour>();

            var year0Lines = new DialogLine[]
            {
                new DialogLine("HELLO", false), 
                new DialogLine("HELLO BACK", true)
            };

            Lines
                .Add(year0Lines);

            var year1Lines = new DialogLine[]
            {
                new DialogLine("HELLO AGAIN", false),
                new DialogLine("HELLO AGAIN BACK", true)
            };

            Lines
                .Add(year1Lines);
        }

        void Start()
        {
            text.text = "";
            ShowYearMessage(0);
        }

        void DisplayText(int index)
        {
            var yearLines = Lines[Year];

            if (yearLines[index].IsSelf)
            {
                SpeakerLeftImage.CrossFadeAlpha(0.25f, 0.5f, false);
                SpeakerRightImage.CrossFadeAlpha(1f, 0.5f, false);

                text.alignment = TextAlignmentOptions.TopRight;
            }
            else
            {
                SpeakerLeftImage.CrossFadeAlpha(1f, 0.5f, false);
                SpeakerRightImage.CrossFadeAlpha(0.25f, 0.5f, false);

                text.alignment = TextAlignmentOptions.TopLeft;
            }

            text.text = "";
            
            StartCoroutine(PrintText(yearLines[LinesPlace].Text));

            if (LinesPlace == yearLines.Length - 1)
            {
                ButtonText.text = "Done";
            }
            else
            {
                ButtonText.text = "Next";
            }
        }

        public void NextText()
        {
            LinesPlace++;

            var yearLines = Lines[Year];

            if (LinesPlace >= yearLines.Length)
            {
                gameObject
                    .SetActive(false);

                Gameworld
                    .StartNewYear(Gameworld.Year + 1);

                return;
            }

            DisplayText(LinesPlace);
        }

        IEnumerator PrintText(string txt)
        {
            foreach (char c in txt)
            {
                text.text += c;
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void ShowYearMessage(int year)
        {
            Year = year;

            gameObject
                .SetActive(true);

            LinesPlace = 0;

            DisplayText(LinesPlace);
        }
    }
}
