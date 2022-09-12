using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gems : MonoBehaviour, ICollectible
{
    public static event HandleGemCollected OnGemsCollected;
    public delegate void HandleGemCollected(ItemData itemData);
    public ItemData gemData;

    public void Collect()
    {
        // Debug.Log("You collected gem");
        Destroy(gameObject);
        OnGemsCollected?.Invoke(gemData);
    }
}
