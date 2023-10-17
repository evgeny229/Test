using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : PickableObject
{
    public int _damage { get; private set; }
    public override void Action(Collider2D collision)
    {
        collision.gameObject.GetComponent<DestroyedObj>().GetDamage(_damage);
        GetComponent<BoxCollider2D>().enabled = false;
    }
    public void SetDamage(int damage) => _damage = damage;
}
