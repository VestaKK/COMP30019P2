using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField] KeyBinder binder;

    // When object is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public KeyCode GetKeyForAction(KeyBindingAction action)
    {
        foreach (KeyBinder.KeyBind keyBind in binder.keyBinds) {
            if (keyBind.Action == action) {
                return keyBind.KeyCode;            
            }
        }
        return KeyCode.None;
    }

    public bool GetKeyDown(KeyBindingAction action) {

        foreach(KeyBinder.KeyBind keyBind in binder.keyBinds) {
            if (keyBind.Action == action)
            {
                return Input.GetKeyDown(keyBind.KeyCode);
            }
        }
        return false;
    }

    public bool GetKey(KeyBindingAction action)
    {
        foreach (KeyBinder.KeyBind keyBind in binder.keyBinds)
        {
            if (keyBind.Action == action)
            {
                return Input.GetKey(keyBind.KeyCode);
            }
        }
        return false;
    }

}
