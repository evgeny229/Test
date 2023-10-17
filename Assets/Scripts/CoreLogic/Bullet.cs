using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage { get; set; }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyedObj Col = collision.transform.GetComponent<DestroyedObj>();
        if (Col != null)
            Col.GetDamage(_damage);
        Destroy(gameObject);
    }
    public void Start()
    {
        Invoke(nameof(DestroyBullet), 10f);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
