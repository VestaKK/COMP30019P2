using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Might honestly be a container for other UI elements on the screen
public class PlayerHUD : UIPanel
{
    [SerializeField] Player _player;
    [SerializeField] private Image hitPanel;
    [SerializeField] private Image healPanel;
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
        if (this.gameObject.activeSelf == false) return;
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
        if (this.gameObject.activeSelf == false) return;
        Color temp = hitPanel.color;
        temp.a = 0.3f;
        hitPanel.color = temp;
        StartCoroutine(HitEffectCoroutine());
    }

    private void Update()
    {
        if (hitPanel.color.a > 0 && !hitPanelCoroutine)
        {
            Color temp = hitPanel.color;
            temp.a = 0;
            hitPanel.color = temp;
            hitPanelCoroutine = false;
        }

        if (true) 
        {
            
        }
    }


    private IEnumerator HitEffectCoroutine() 
    {
        hitPanelCoroutine = true;
        while (hitPanel.color.a > 0.05)
        {
            Color temp = hitPanel.color;
            temp.a = temp.a - 0.3f * Time.deltaTime;
            hitPanel.color = temp;
            yield return null;
        }

        Color final = hitPanel.color;
        final.a = 0;
        hitPanel.color = final;

        hitPanelCoroutine = false;
        yield return null;
    }
}
