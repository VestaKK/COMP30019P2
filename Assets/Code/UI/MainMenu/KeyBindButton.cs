using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindButton : MonoBehaviour
{
    [SerializeField] private Text ButtonText;
    public InputAction action;

    private void OnEnable()
    {
        OptionsMenu.OnKeyRebind += UpdateText;
        ButtonText.text = InputManager.GetKeyForAction(action).ToString();
    }

    private void OnDisable()
    {
        OptionsMenu.OnKeyRebind -= UpdateText;
    }

    void UpdateText(InputAction action) {
        if (this.action == action)
            ButtonText.text = InputManager.GetKeyForAction(this.action).ToString();
    }
}
