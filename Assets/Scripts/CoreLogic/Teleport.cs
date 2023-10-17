using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Teleport SecTeleport;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (SecTeleport != null)
            collision.transform.position = SecTeleport.transform.position;
    }
}
