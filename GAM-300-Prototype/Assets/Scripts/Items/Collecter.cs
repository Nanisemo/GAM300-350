using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    // NOTE to Valerie/Liyi: Change as you need but currently item picking is set to ontrigger
    private void OnTriggerEnter(Collider other) {
        ICollectible collectible = other.GetComponent<ICollectible>();
        if(collectible != null)
            collectible.Collect();
    }
}
