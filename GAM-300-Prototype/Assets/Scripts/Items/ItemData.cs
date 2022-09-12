using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemID;
    public string Name;
    public string ItemTypes;
    public string Description;
    public Sprite Icon;
}
