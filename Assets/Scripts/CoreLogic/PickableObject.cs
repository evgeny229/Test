using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickableObject : MonoBehaviour
{
    private bool isUsed;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isUsed) return;
        if (collision.name == "player")
        {
            Destroy(gameObject);
            Action(collision);
            isUsed = true;
        }
    }
    public virtual void Action(Collider2D collision)
    {

    }
}
