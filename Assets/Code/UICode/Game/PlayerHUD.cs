using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Might honestly be a container for other UI elements on the screen
public class PlayerHUD : UIPanel
{
    [SerializeField] PlayerHealthBar healthBar;

    public override void Initialise()
    {
    }
}
