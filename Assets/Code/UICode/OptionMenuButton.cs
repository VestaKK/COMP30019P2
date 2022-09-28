using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuButton : MonoBehaviour
{
    [SerializeField] private Text ButtonText;
    [SerializeField] private InputAction action;

    private void Start()
    {
        ButtonText.text = InputManager.instance.GetKeyForAction(this.action).ToString();
    }

    private void OnEnable()
    {
        OptionsMenu.OnKeyRebind += UpdateText;
    }

    private void OnDisable()
    {
        OptionsMenu.OnKeyRebind -= UpdateText;
    }

    void UpdateText(InputAction action) {
        if (this.action == action)
            ButtonText.text = InputManager.instance.GetKeyForAction(this.action).ToString();
    }
}
