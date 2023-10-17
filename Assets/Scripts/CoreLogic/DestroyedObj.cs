using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class DestroyedObj : MonoBehaviour
{
    public int health = 3;
    public bool IsDead = false;
    public Transform HealthText;

    protected MusicableObject MusicableObject;

    private void Start()
    {
        MusicableObject = GetComponent<MusicableObject>();
    }
    public virtual void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            HealthLessZero();
    }

    protected virtual void HealthLessZero()
    {
        health = 0;
        MusicableObject?.PlayOneShot(1);
    }
}
