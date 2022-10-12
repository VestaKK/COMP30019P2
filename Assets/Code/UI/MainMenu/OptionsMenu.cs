using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : UIPanel
{
    [SerializeField] GameObject WaitForInputScreen;
    [SerializeField] Button backButton;

    public delegate void OptionMenuEvent(InputAction action);
    public static event OptionMenuEvent OnKeyRebind;

    public override void Initialise()
    {
        backButton.onClick.AddListener(() => UIManager.instance.ShowLast());
    }

    public void Rebind(string actionString) {
        foreach (InputAction action in System.Enum.GetValues(typeof(InputAction)))
        {
            if (action.ToString().Equals(actionString))
                StartCoroutine(RebindCoroutine(action));
        }
    }

    private IEnumerator RebindCoroutine(InputAction action)
    { 
        WaitForInputScreen.SetActive(true);
        while(true)
        {
            if (Input.anyKeyDown)
            {
                KeyCode keyPressed = KeyCode.None;

                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(key))
                    {
                        keyPressed = key;
                        break;
                    }
                }

                WaitForInputScreen.SetActive(false);
                InputManager.RebindKey(action, keyPressed);
                OnKeyRebind.Invoke(action);
                break;
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }
}

