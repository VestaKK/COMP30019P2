using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : UIPanel {

    [SerializeField] Button closeButton;
    [SerializeField] Button quitButton;
    bool hidden = true;

    public override void Initialise()
    {
        closeButton.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("UIClick");
            UIManager.instance.ShowLast();
            GameManager.UnpauseGame();
        });

        quitButton.onClick.AddListener(() => {
            FindObjectOfType<AudioManager>().Play("UIClick");
            SceneManager.LoadScene(0);
        });
    }

}
