using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField] KeyBinder binder;

    // Singleton Stuff
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Makes sure this isn't unloaded when loading a new scene
            DontDestroyOnLoad(this);    
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    public KeyCode GetKeyForAction(InputAction action)
    {
        foreach (KeyBinder.KeyBind keyBind in binder.keyBinds) {
            if (keyBind.Action == action) {
                return keyBind.KeyCode;            
            }
        }
        return KeyCode.None;
    }

    public bool GetKeyDown(InputAction action) {
        foreach(KeyBinder.KeyBind keyBind in binder.keyBinds) {
            if (keyBind.Action == action)
            {
                return Input.GetKeyDown(keyBind.KeyCode);
            }
        }
        return false;
    }

    public bool GetKey(InputAction action)
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

    public void RebindKey(InputAction action, KeyCode newKey) {
        foreach (KeyBinder.KeyBind keyBind in binder.keyBinds)
        {
            if (keyBind.Action == action)
            {
                keyBind.KeyCode = newKey;
                return;
            }
        }
    }
}
