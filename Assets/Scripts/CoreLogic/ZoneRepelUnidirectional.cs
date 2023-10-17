using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRepelUnidirectional : Zone
{
    public void Update()
    {
        RefreshForceInZone();//force
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
            
            if (transform.eulerAngles.z == 0)
                ResultVector = new Vector2(1, 0);
            else if (transform.eulerAngles.z == 90)
                ResultVector = new Vector2(0, 1);
            else if (transform.eulerAngles.z == 180)
                ResultVector = new Vector2(-1, 0);
            else if(transform.eulerAngles.z == 270)
                ResultVector = new Vector2(0, -1); 
            else
            {
                float x = 1;
                if (transform.eulerAngles.z > 90f && transform.eulerAngles.z < 270f)
                    x = -1;
                float y = x * Mathf.Tan(transform.eulerAngles.z / 180f * 3.14f);
                ResultVector = new Vector2(x, y);
            }
            allObjectsInZone[i].AddForce(ResultVector.normalized * power * kp);
        }
    }
}
