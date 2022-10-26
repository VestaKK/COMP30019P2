using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : UIPanel
{

    [SerializeField] Button quitButton;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text timeText;

    public override void Initialise()
    {
        quitButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenuScene"));
    }

    public void SetStats(int score, int levelsCleared, float time) 
    {
        scoreText.text = "SCORE:\n" + score.ToString();
        levelText.text = "LEVELS:\n" + levelsCleared.ToString();
        timeText.text = "TIME:\n" + time.ToString();
    }
}
