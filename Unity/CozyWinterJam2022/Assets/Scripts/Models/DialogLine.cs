namespace HNS.CozyWinterJam2022.Models
{
    public class DialogLine
    {
        public DialogLine(string text, bool isSelf)
        {
            Text = text;
            IsSelf = isSelf;
        }

        public string Text { get; set; }

        public bool IsSelf { get; set; }
    }
}