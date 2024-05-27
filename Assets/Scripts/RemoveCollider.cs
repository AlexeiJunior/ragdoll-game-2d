using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCollider : MonoBehaviour
{
    public List<Transform> ignoreCollisionList;

    void Start()
    {
        foreach (Transform child in ignoreCollisionList)
        {
            Physics2D.IgnoreCollision(child.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }
}
