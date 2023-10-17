using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRepelRadial : Zone
{
    public void Update()
    {
        RefreshForceInZone();
    }
    public void RefreshForceInZone()
    {
        for (int i = 0; i < allObjectsInZone.Count; i++)
        {
            if (allObjectsInZone[i] == null) continue;
            float dist = (allObjectsInZone[i].transform.position - transform.position).magnitude;
            float kp = dist / NeedDistanceToCenter;
            if (kp > 1)
                kp = 1;
            if (kp < 0.25f)
                kp = 0.25f;
            Vector2 ResultVector = new Vector2(transform.position.x - allObjectsInZone[i].transform.position.x, transform.position.y - allObjectsInZone[i].transform.position.y);
            allObjectsInZone[i].AddForce(ResultVector.normalized * power * kp);
        }
    }
}
