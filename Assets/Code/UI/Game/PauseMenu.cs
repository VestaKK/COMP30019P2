using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : UIPanel {

    [SerializeField] Button quitButton;
    bool hidden = true;

    public override void Initialise()
    {
        quitButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenuScene"));
    }

}
