using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPush : MonoBehaviour
{
    [SerializeField] float forceMagnitude = 1f;

    private void OnControllerColliderHit(ControllerColliderHit hitInfo)
    {
        Rigidbody rb = hitInfo.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            if (hitInfo.gameObject.CompareTag("Cart Handle"))
            {
                Vector3 forceDrirection = hitInfo.moveDirection;
                forceDrirection.y = 0;
                forceDrirection.Normalize();

                rb.velocity = forceDrirection * forceMagnitude;

                print(rb.name);
            }
        }
    }
}
