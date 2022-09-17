using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMove : MonoBehaviour, Interactable
{
    public GameObject cartBase;
    public Vector3 distanceToMove; // Z for front/back (negative Z), X for right/left (negative X).
    public float moveSpeed = 3f;

    bool colliding;

    public IEnumerator Interact(Transform player)
    {
        Rigidbody rb = cartBase.GetComponent<Rigidbody>();
        Vector3 targetPos = cartBase.transform.position + distanceToMove;

        while (cartBase.transform.position != targetPos)
        {
            rb.MovePosition(Vector3.MoveTowards(cartBase.transform.position, targetPos, moveSpeed * Time.deltaTime));
            player.position = cartBase.transform.position;

            yield return null;
        }
        cartBase.transform.position = targetPos;

    }

    //private void OnCollisionEnter(Collision hitInfo)
    //{
    //    if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {

    //    }
    //}
}
