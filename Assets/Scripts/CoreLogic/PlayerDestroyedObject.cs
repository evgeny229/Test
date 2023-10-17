using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerDestroyedObject : DestroyedObj
{
    public Text Health;//player
    private EventProvider EventProvider;//Player
    [Inject]
    public void Construct(EventProvider eventProvider)//player
    {
        EventProvider = eventProvider;
    }
    private void Start()//player
    {
        PlayMusicOnPlayer();
    }
    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        if (damage < 0)//player
            MusicableObject?.PlayOneShot(3);
        Health.text = "" + health;
    }

    protected override void HealthLessZero()
    {
        base.HealthLessZero();
        EventProvider.PlayerIsDead.Invoke();
    }
    private void PlayMusicOnPlayer()//player
    {
        MusicableObject.PlayOneShot(1);
    }
    private void SetHealth(int health)//player
    {
        if (Health != null)
            Health.text = health + "";
        this.health = health;
    }
}
