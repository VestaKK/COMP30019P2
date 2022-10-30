using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Relic", fileName = "Relic")]
public class Item : ScriptableObject
{
    [SerializeField] public int id=0;
    [SerializeField] public string name = "itemName";
    [SerializeField] public string description = "description";
    [SerializeField] public Sprite icon;
}
