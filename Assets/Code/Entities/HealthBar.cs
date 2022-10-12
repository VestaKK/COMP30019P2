using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    [SerializeField] private Image _foreground;
    [SerializeField] private Image _background;

    [SerializeField] private EntityController _entity;
    public void UpdateHealthbar() {
        _foreground.fillAmount = _entity.Health / _entity.MaxHealth;
    }
}