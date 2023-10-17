using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public abstract class Level : MonoBehaviour
{
    public Transform lastPoint;
    public Transform Center;
    public float CameraScale;

    public virtual void StartLevel()
    {
    }
}
