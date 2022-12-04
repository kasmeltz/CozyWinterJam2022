namespace HNS.CozyWinterJam2022.Behaviours
{
    using HNS.CozyWinterJam2022.Models;
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

        public Sprite SantaSprite;
        public Sprite AdvisorSprite;
        public Sprite ElfHatSprite;

        public List<DialogLine[]> Lines { get; set; }

        protected GameworldBehaviour Gameworld { get; set; }

        protected int Year { get; set; }

        private void Awake()
        {
            Lines = new List<DialogLine[]>();

            Gameworld = FindObjectOfType<GameworldBehaviour>();

            var year0Lines = new DialogLine[]
            {
                new DialogLine("<i>*Once upon a time, on one cosy day at the North Pole.*</i>", false, null, null),
                new DialogLine("... You want to do WHAT?!", true, SantaSprite, AdvisorSprite),
                new DialogLine("Deliver Presents to all the children of the world.", false, SantaSprite, AdvisorSprite),
                new DialogLine("THE WORLD?! Do you have any idea how many children there are in the world? THE PLANNING, the Infastructure!", true, SantaSprite, AdvisorSprite),
                new DialogLine("You have lost your mind!", true, SantaSprite, AdvisorSprite),
                new DialogLine("Perhaps...", false, SantaSprite, AdvisorSprite),
                new DialogLine("... ", false, SantaSprite, AdvisorSprite),
                new DialogLine("... But I know I want to do this, Scribbles. Call me mad all you like. I at the very least know you are just the elf I need to make this possible.", false, SantaSprite, AdvisorSprite),
                new DialogLine("So, are you in? Or do you I have to bring up the fact I saved you from those candywolves all those years ago?", false, SantaSprite, AdvisorSprite),
                new DialogLine("...", true, SantaSprite, AdvisorSprite),
                new DialogLine("... Ah featherwinkles, your machinations will one day be my undoing, Mr.C! And I will have you know you extended that acquired favor already many times over in the past years, do not you dare bring it up again!", true, SantaSprite, AdvisorSprite),
                new DialogLine("...But fine...", true, SantaSprite, AdvisorSprite),
                new DialogLine("... We begin from the start. We begin small, you leave most things to me, and I will leave you to worry how you intend to break space and somehow find the time to visit every home in the world individually over the course of a single eve. Got it?", true, SantaSprite, AdvisorSprite),
                new DialogLine("<i>TUTORIAL\n*Later that Day...*</i>", false, null, null),
                new DialogLine("Next! ... Ah, the new Overseer. Let me go over your list of tasks.", true, null, AdvisorSprite),
                new DialogLine("OH... <b>You are the one that will have to make sure that by the end of the year the big man gets his presents to deliver</i>. Otherwise peoples <b>Christmas Joy</b> will go down and this fragile dream we have created will be ruined! ... FOREVER... No pressure.", true, null, AdvisorSprite),
                new DialogLine("Now, go make us 100 Presents to start with. Lets see what you are worth.", true, null, AdvisorSprite),
            };

            Lines
                .Add(year0Lines);

            var year1Lines = new DialogLine[]
            {
                new DialogLine("This is year 2", false, null, null),
            };

            Lines
                .Add(year1Lines);

            var year2Lines = new DialogLine[]
            {
                new DialogLine("This is year 3", false, null, null),
            };

            Lines
                .Add(year2Lines);
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

            text.text = yearLines[LinesPlace].Text;

            if (!yearLines[index].LeftCharacter)
            {
                SpeakerLeftImage.gameObject.SetActive(false);
            }
            else
            {
                SpeakerLeftImage.gameObject.SetActive(true);
                SpeakerLeftImage.sprite = yearLines[index].LeftCharacter;
            }

            if (!yearLines[index].RightCharacter)
            {
                SpeakerRightImage.gameObject.SetActive(false);
            }
            else
            {
                SpeakerRightImage.gameObject.SetActive(true);
                SpeakerRightImage.sprite = yearLines[index].RightCharacter;
            }

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
