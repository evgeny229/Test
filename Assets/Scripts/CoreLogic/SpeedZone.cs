using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedZone : Zone
{
    public void Update()
    {
        RefreshSpeedInZone();//speed
    }
    public void RefreshSpeedInZone()
    {
        float dist = (Player.position - transform.position).magnitude;
        float kp = dist / NeedDistanceToCenter;
        if (kp > 1)
            kp = 1;
        ///
    }
}
