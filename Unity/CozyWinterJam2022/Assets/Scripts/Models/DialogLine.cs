namespace HNS.CozyWinterJam2022.Models
{
    using UnityEngine;

    public class DialogLine
    {
        public DialogLine(string text, bool isSelf, Sprite leftCharacter, Sprite rightCharacter)
        {
            Text = text;
            IsSelf = isSelf;
            LeftCharacter = leftCharacter;
            RightCharacter = rightCharacter;
        }

        public string Text { get; set; }
        public bool IsSelf { get; set; }
        public Sprite LeftCharacter { get; set; }
        public Sprite RightCharacter { get; set; }
    }
}