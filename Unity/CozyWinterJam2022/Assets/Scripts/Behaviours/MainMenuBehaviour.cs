using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject MenuButtons;
    public GameObject CreditsMenu;

    float TargetButtonsWidth = 1f;
    float TargetCreditsHeight = -444;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MenuButtons.transform.localScale = Vector3.Lerp(MenuButtons.transform.localScale, new Vector3(TargetButtonsWidth, 1, 1), 0.1f);
        CreditsMenu.transform.localPosition = Vector3.Lerp(CreditsMenu.transform.localPosition, new Vector3(0, TargetCreditsHeight, 0), 0.1f);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void CreditsButton()
    {
        TargetButtonsWidth = 0f;
        TargetCreditsHeight = -46;
    }

    public void CloseCreditsButtons()
    {
        TargetButtonsWidth = 1f;
        TargetCreditsHeight = -444;
    }
}
