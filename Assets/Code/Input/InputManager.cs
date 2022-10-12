using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
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

    public static bool GetKey(InputAction action) {
        return instance.LocalGetKey(action);
    }

    public static bool RebindKey(InputAction action, KeyCode newKey) {
        return instance.LocalRebindKey(action, newKey);
    }

    public static KeyCode GetKeyForAction(InputAction action) {
        return instance.LocalGetKeyForAction(action);
    }

    public static bool GetKeyDown(InputAction action) {
        return instance.LocalGetKeyDown(action);
    }

    private KeyCode LocalGetKeyForAction(InputAction action)
    {
        foreach (KeyBinder.KeyBind keyBind in binder.keyBinds) {
            if (keyBind.Action == action) {
                return keyBind.KeyCode;            
            }
        }
        return KeyCode.None;
    }

    private bool LocalGetKeyDown(InputAction action) {
        foreach(KeyBinder.KeyBind keyBind in binder.keyBinds) {
            if (keyBind.Action == action)
            {
                return Input.GetKeyDown(keyBind.KeyCode);
            }
        }
        return false;
    }

    private bool LocalGetKey(InputAction action)
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

    private bool LocalRebindKey(InputAction action, KeyCode newKey) {
        foreach (KeyBinder.KeyBind keyBind in binder.keyBinds)
        {
            if (keyBind.Action == action)
            {
                keyBind.KeyCode = newKey;
                return true;
            }
        }

        return false;
    }

}
