using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBehaviour : MonoBehaviour
{
    public TMP_Text ButtonText;
    public Image SpeakerLeftImage;
    public Image SpeakerRightImage;
    public TextMeshProUGUI text;
    public string[] Lines;
    public bool[] IsSelf;
    int LinesPlace = 0;

    void Start()
    {
        text.text = "";
        DisplayText(LinesPlace);
    }

    void DisplayText(int index)
    {
        if (IsSelf[index])
        {
            SpeakerLeftImage.CrossFadeAlpha(0.25f, 0.5f, false);
            SpeakerRightImage.CrossFadeAlpha(1f, 0.5f, false);

            text.alignment = TMPro.TextAlignmentOptions.TopRight;
        }
        else
        {
            SpeakerLeftImage.CrossFadeAlpha(1f, 0.5f, false);
            SpeakerRightImage.CrossFadeAlpha(0.25f, 0.5f, false);

            text.alignment = TMPro.TextAlignmentOptions.TopLeft;
        }

        text.text = "";
        StartCoroutine(PrintText(Lines[LinesPlace]));

        if (LinesPlace == Lines.Length - 1)
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

        if(LinesPlace >= Lines.Length)
        {
            gameObject
                .SetActive(false);

            return;
        }

        DisplayText(LinesPlace);
    }

    IEnumerator PrintText(string txt)
    {
        foreach(char c in txt)
        {
            text.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
