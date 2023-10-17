using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObj : MonoBehaviour
{
    void Start()
    {
        float needScale = transform.localScale.x;
        float needScaleValue = needScale * 1.1f;
        float value = 0;
        StartCoroutine(b());
        IEnumerator b()
        {
            while (value < needScale)
            {
                value = Mathf.Lerp(value, needScaleValue, 10f * Time.deltaTime);
                transform.localScale = new Vector3(value, value, 1);
                yield return null;
            }
            transform.localScale = new Vector3(needScale, needScale, 1);
        }
    }
}
