using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject blackOutSquare;

    [SerializeField] private UIPanel startPanel;

    [SerializeField] private UIPanel[] panels;

    [SerializeField] private UIPanel currentPanel;

    public bool fadedOut = false;

    private readonly Stack<UIPanel> panelStack = new();

    // Not using a strict singleton (One UI Manager Per Scene)
    private void Awake()
    {
        // Definitely a cheese hack to avoid PauseMenu
        // Freezing the game when entering twice
        Time.timeScale = 1;
        if (instance == null)
        {
            instance = this;

            foreach (UIPanel uIPanel in panels)
            {
                uIPanel.Initialise();
                uIPanel.Hide();
            }

            startPanel.Show();
            currentPanel = startPanel;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = 5) {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if(fadeToBlack) {
            while(blackOutSquare.GetComponent<Image>().color.a < 1) {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            fadedOut = true;
        } else {
            while (blackOutSquare.GetComponent<Image>().color.a > 0) {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
            fadedOut = false;
        }
    }


    public T Get<T>() where T : UIPanel
    {
        foreach(UIPanel uIPanel in panels) 
        { 
            if (uIPanel is T tPanel) return tPanel;
        }
        return null;
    }

    public void Show<T>(bool remember = true) where T : UIPanel
    {
        foreach (UIPanel uIPanel in panels)
        {
            if (uIPanel is T) 
            {
                if (currentPanel != null)
                {
                    if (remember)
                    {
                        panelStack.Push(instance.currentPanel);
                    }

                    currentPanel.Hide();
                }

                uIPanel.Show();
                currentPanel = uIPanel;
            }
        }
    }

    public void Show(UIPanel uIPanel, float fadeSpeed, bool remember = true) 
    {
        if (currentPanel != null)
        {
            if (remember)
            {
                panelStack.Push(currentPanel);
            }

            if(fadeSpeed != 0f) {
                StartCoroutine(FadeBlackOutSquare(true, fadeSpeed));
            }
            while(fadedOut) {}
            currentPanel.Hide();
        }
        if(fadeSpeed != 0f) {
            StartCoroutine(FadeBlackOutSquare(false, fadeSpeed));
        }
        while(!fadedOut) {}
        uIPanel.Show();
        currentPanel = uIPanel;
    }

    public void Show(UIPanel uIPanel, bool remember = true) {
        Show(uIPanel, 0f, remember);
    }

    public void ShowLast() 
    {
        if (panelStack.Count != 0)
        {
            Show(panelStack.Pop());
        }
    }

    public void HideAll() 
    {
        foreach (UIPanel uIPanel in panels)
        {
            uIPanel.Hide();
        }
    }

    // Ideally we fire an Event OnPauseGame that's
    // Controlled by a game manager But this will do for now
    private void Update()
    {
        if (InputManager.GetKeyDown(InputAction.Pause)) 
        {
            UIPanel pauseMenu = Get<PauseMenu>();

            if (pauseMenu != null)
            {
                if (!pauseMenu.gameObject.activeSelf)
                {
                    Show(pauseMenu, true);
                }
                else
                {
                    ShowLast();
                }
            }   
        }

        if(InputManager.GetKeyDown(KeyCode.A)) {
            StartCoroutine(FadeBlackOutSquare(true, 3));
        }
        if(InputManager.GetKeyDown(KeyCode.S)) {
            StartCoroutine(FadeBlackOutSquare(false, 3));
        }
    }
}
