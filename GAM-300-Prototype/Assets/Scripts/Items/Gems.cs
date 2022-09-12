using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gems : MonoBehaviour, ICollectible
{
    public static event Action OnGemsCollected;

    public void Collect()
    {
        Debug.Log("You collected a coin");
        Destroy(gameObject);
        OnGemsCollected?.Invoke();
        // Gems.OnGemsCollected = do something
    }
}
