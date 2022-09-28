using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public GameObject playerGO;

    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(transform.position.x - playerGO.transform.position.x,
            transform.position.y - playerGO.transform.position.y,
            transform.position.z - playerGO.transform.position.z);
    }

    private void Update()
    {
        Vector3 finalPos = playerGO.transform.position;
        foreach (var enemy in GlobalBool.enemiesInCombat)
        {
            finalPos.x += enemy.transform.position.x;
            finalPos.y += enemy.transform.position.y;
            finalPos.z += enemy.transform.position.z;
        }
        finalPos /= GlobalBool.enemiesInCombat.Count + 1;
        transform.position = finalPos + offset;
    }
}
