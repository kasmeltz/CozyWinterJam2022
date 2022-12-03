using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBehaviour : MonoBehaviour
{
    public Image SpeakerLeftImage;
    public Image SpeakerRightImage;
    public TMPro.TextMeshProUGUI text;
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
    }

    public void NextText()
    {
        LinesPlace++;
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
