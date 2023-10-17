using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNTDestroyedObject : DestroyedObj
{
    Tnt Tnt;
    private void Start()
    {
        Tnt = GetComponent<Tnt>();
    }
    protected override void HealthLessZero()
    {
        base.HealthLessZero();
        Tnt.Explosive();
    }
}
