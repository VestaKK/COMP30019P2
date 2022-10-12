using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : UIPanel {
    [SerializeField] Image _splashImage;
    [SerializeField] float _splashDisplayTimeSeconds;
    [SerializeField] float fadeTime;
    private float _futureTime;
    private float _totalTime;
    public override void Initialise()
    {   
    }

    public override void Show()
    {
        base.Show();
        _futureTime = Time.deltaTime + _splashDisplayTimeSeconds;

    }

    void Update() {
        if(_totalTime > _futureTime) {
            Display();
            return;
        }
        _totalTime += Time.deltaTime;
    }

    private void Display() {
        UIManager.instance.Show<MainMenu>();
    }
}