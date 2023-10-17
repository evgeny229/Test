using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Zenject;
using System;

public partial class CameraInGameScene : MonoBehaviour
{
    public Rigidbody2D CameraRigidbody2D;

    public void AddForce(Vector2 force)
    {
        CameraRigidbody2D.AddForce(force);
    }
}
