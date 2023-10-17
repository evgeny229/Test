using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : PickableObject
{
    private int _addHeathCount;
    public override void Action(Collider2D player)
    {
        player.transform.GetComponent<DestroyedObj>().GetDamage(-_addHeathCount);
    }
    public void SetAddHealthCount(int addHealth)
    {
        _addHeathCount = addHealth;
    }
}
