using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBinder", menuName = "KeyBinder")]
public class KeyBinder : ScriptableObject
{
    [System.Serializable]
    public class KeyBind
    {
        public KeyBindingAction Action;
        public KeyCode KeyCode;
    }

    public KeyBind[] keyBinds;
}
