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
    [SerializeField] private ProgressBar healthBar;

    bool hitPanelCoroutine = false;
    bool healthUpdatePending = false;
    float healthPercentageToUpdate = 0;
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
        _player.OnHealthUpdate += UpdateHealthBar;
        _player.OnHeal += HUDHealEffect;
    }

    public void UpdateHealthBar(float healthPercentage) 
    {
        if (this.gameObject.activeSelf)   
            healthBar.SetProgress(healthPercentage);
        else 
            healthBar.SetProgressInstant(healthPercentage);
    }

    private void OnDestroy()
    {
        PlayerInventory.OnInventoryUpdate -= DisplayItemPickupText;
        GameManager.OnEnterExitRoom -= DisplayInteractiveText;
        GameManager.OnExitExitRoom -= DropInteractiveText;
        _player.OnTakeDamage -= HUDDamageEffect;
        _player.OnHealthUpdate -= UpdateHealthBar;
        _player.OnHeal -= HUDHealEffect;
    }

    private void DisplayItemPickupText(ItemSlot itemSlot) 
    {
        textDisplay.DisplayFadingText("+ " + itemSlot.item.name, 3f);
    }

    private void Update()
    {

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

    private void HUDHealEffect() 
    {
        healPanel.FadeEffect();
    }
}
