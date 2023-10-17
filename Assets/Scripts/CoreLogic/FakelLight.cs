using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FakelLight : MonoBehaviour
{
    private Light2D currLight;
    private float MaxValue;
    private float MaxValue2;
    private float MinValue;
    private float MinValue2;
    private float speedChange = 0.75f;
    private bool up;
    void Start()
    {
        MaxValue = currLight.pointLightOuterRadius * 1.1f;
        MinValue = currLight.pointLightOuterRadius / 1.05f;
        MaxValue2 = MaxValue * 1.1f;
        MinValue2 = MinValue / 1.1f;
        if(Random.Range(0,2) == 0)
            up = true;
        currLight = GetComponent<Light2D>();
    }
    void Update()
    {
        UpdateLightStrength();
    }

    private void UpdateLightStrength()
    {
        if (up)
        {
            currLight.pointLightOuterRadius = Mathf.Lerp(currLight.pointLightOuterRadius, MaxValue2, speedChange * Time.deltaTime);
            if (currLight.pointLightOuterRadius > MaxValue)
            {
                up = false;
                RandomSpeedChange();
            }
        }
        else
        {
            currLight.pointLightOuterRadius = Mathf.Lerp(currLight.pointLightOuterRadius, MinValue2, speedChange * Time.deltaTime);
            if (currLight.pointLightOuterRadius < MinValue)
            {
                up = true;
                RandomSpeedChange();
            }
        }
    }
    private void RandomSpeedChange()
    {
        speedChange = Random.Range(0.5f, 1f);
    }
}
