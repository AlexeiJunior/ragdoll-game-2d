using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThings : MonoBehaviour
{
    private HingeJoint2D tempHinge;
    private GameObject objCollision;
    public PlayerConfig player;

    void Update()
    {
        if (!Input.GetMouseButton(0) && objCollision)
        {
            Destroy(tempHinge);
            player.playerGrabbing = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!player.playerGrabbing && collision.gameObject.layer == 9 && Input.GetMouseButton(0))
        {
            player.useLeftHand = gameObject.name == "lefthand";
            player.playerGrabbing = true;
            objCollision = collision.gameObject;
            tempHinge = collision.gameObject.AddComponent<HingeJoint2D>();
            tempHinge.connectedBody = gameObject.GetComponent<Rigidbody2D>();
            tempHinge.autoConfigureConnectedAnchor = false;
            tempHinge.anchor = collision.gameObject.transform.InverseTransformPoint(collision.contacts[0].point);
            tempHinge.connectedAnchor = Vector2.zero;
        }
    }
}