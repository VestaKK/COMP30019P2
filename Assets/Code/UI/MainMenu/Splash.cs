using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Splash : UIPanel {
    public TMP_Text _splashText;
    [SerializeField] float _splashDisplayTimeSeconds;
    public float fadeTime;
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
        UIManager.instance.Show(UIManager.instance.Get<MainMenu>(), fadeTime != 0f ? fadeTime : 3);
    }
}