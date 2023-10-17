using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDestroyedObject : DestroyedObj
{
    protected override void HealthLessZero()
    {
        base.HealthLessZero();
        Destroy(gameObject);
    }
}
