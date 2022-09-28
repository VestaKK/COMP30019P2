using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsMenu : MonoBehaviour
{
    public delegate void OptionMenuEvent(InputAction action);
    public static event OptionMenuEvent OnKeyRebind;

    public GameObject WaitForInputScreen;

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
                InputManager.instance.RebindKey(action, keyPressed);
                OnKeyRebind(action);
                break;
            }
            yield return null; // wait until next frame, then continue execution from here (loop continues)
        }
    }
}

