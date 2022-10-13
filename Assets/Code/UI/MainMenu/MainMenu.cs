using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : UIPanel
{
    [SerializeField] Button playButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button quitButton;

    public override void Initialise()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene("NewControllerScene"));
        optionsButton.onClick.AddListener(() => UIManager.instance.Show<OptionsMenu>(true));
        quitButton.onClick.AddListener(() => Application.Quit());
    }
}
