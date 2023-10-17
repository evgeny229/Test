using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Tnt : MonoBehaviour
{
    public int damage;
    private bool IsExplosed;
    private Transform Player;
    private Rigidbody2D currBody;
    private GameObject imageExplosion;
    private ParticleSystem particleExplosion;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private MediatorCore MediatorCore;
    [Inject]
    public void Construct(Transform player,
        MediatorCore mediatorCore)
    {
        Player = player;
        MediatorCore = mediatorCore;
    }
    private void Start()
    {
        StartCoroutine(WaitForSecondsAndDestroy());
        currBody = GetComponent<Rigidbody2D>();
        imageExplosion = transform.GetChild(1).gameObject;
        particleExplosion = transform.GetChild(2).GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    public void Explosive()
    {
        if (IsExplosed) return;
        IsExplosed = true;
        circleCollider.enabled = true;
        boxCollider.enabled = false;
        gameObject.layer = 9;
        imageExplosion.SetActive(true);
        particleExplosion.Play();
        currBody.velocity = Vector2.zero;
        currBody.gravityScale = 0;
        AddCameraForce();
    }


    private void AddCameraForce()
    {
        Vector2 v = Player.position - transform.position;
        v = v.normalized / v.magnitude;
        if (v.magnitude > 5)
            v = v.normalized * 5;
        MediatorCore.AddCameraForce(v * 10000f);
    }
    private IEnumerator WaitForSecondsAndDestroy()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);
            transform.GetChild(3).GetChild(i).gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        Explosive();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyedObj destroyedObj = collision.GetComponent<DestroyedObj>();
        destroyedObj?.GetDamage(damage);
    }
}
