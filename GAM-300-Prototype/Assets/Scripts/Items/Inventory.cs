using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItems> inventory = new List<InventoryItems>();
    private Dictionary<ItemData, InventoryItems> itemDictionary = new Dictionary<ItemData, InventoryItems>();

    private void OnEnable()
    {
        Gems.OnGemsCollected += Add;
    }

    private void OnDisable()
    {
        
    }

    public void Add(ItemData item)
    {
        if (itemDictionary.TryGetValue(item, out InventoryItems invitems))
        {
            invitems.AddToStack();
            Debug.Log($"{item.Name} total stack is now {invitems.stackSize}");
        }
            
        else
        {
            InventoryItems newItem = new InventoryItems(item);
            inventory.Add(newItem);
            itemDictionary.Add(item, newItem);
            Debug.Log($"Added {invitems.name} to the inv for the first time");
        }
    }

    public void Remove(ItemData item)
    {
        if (itemDictionary.TryGetValue(item, out InventoryItems invitem))
        {
            invitem.RemoveFromStack();
            if(invitem.stackSize == 0)
            {
                inventory.Remove(invitem);
                itemDictionary.Remove(item);
            }
        }
    }
}
