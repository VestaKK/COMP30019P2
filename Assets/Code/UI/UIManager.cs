using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private UIPanel startPanel;

    [SerializeField] private UIPanel[] panels;

    [SerializeField] private UIPanel currentPanel;

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

    public void Show(UIPanel uIPanel, bool remember = true) 
    {
        if (currentPanel != null)
        {
            if (remember)
            {
                panelStack.Push(currentPanel);
            }

            currentPanel.Hide();
        }

        uIPanel.Show();
        currentPanel = uIPanel;
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
    }
}
