using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Might honestly be a container for other UI elements on the screen
public class PlayerHUD : UIPanel
{
    [SerializeField] Player _player;
    [SerializeField] private PanelFadeScreen hitPanel;
    [SerializeField] private PanelFadeScreen healPanel;
    [SerializeField] private TextDisplay textDisplay;

    bool hitPanelCoroutine = false;

    public override void Initialise()
    {
        PlayerInventory.OnInventoryUpdate += DisplayItemPickupText;
        GameManager.OnEnterExitRoom += DisplayInteractiveText;
        GameManager.OnExitExitRoom += DropInteractiveText;
    }

    public void LinkPlayer(Player player) 
    {
        _player = player;
        _player.OnTakeDamage += HUDDamageEffect;
    }

    private void OnDestroy()
    {
        PlayerInventory.OnInventoryUpdate -= DisplayItemPickupText;
        GameManager.OnEnterExitRoom -= DisplayInteractiveText;
        GameManager.OnExitExitRoom -= DropInteractiveText;
        _player.OnTakeDamage -= HUDDamageEffect;
    }

    private void DisplayItemPickupText(ItemSlot itemSlot) 
    {
        textDisplay.DisplayFadingText("+ " + itemSlot.item.name, 3f);
    }

    private void DisplayInteractiveText() 
    {
        if (GameManager.inExitRoom) 
        {
            textDisplay.DisplayInteractiveText("PRESS E TO PROCEED");
        }
    }

    private void DropInteractiveText()
    {
        if (!GameManager.inExitRoom)
        {
            textDisplay.DropInteractiveText();
        }
    }

    private void HUDDamageEffect() 
    {
        hitPanel.FadeEffect();
    }
}
