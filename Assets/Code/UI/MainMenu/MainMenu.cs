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
        playButton.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("UIClick");
            SceneManager.LoadScene("DungeonMain");
        });
        optionsButton.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("UIClick");
            UIManager.instance.Show<OptionsMenu>(true);
        });
        quitButton.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("UIClick");
            Application.Quit();
        });
    }
}
