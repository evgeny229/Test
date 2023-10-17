using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraZone : MonoBehaviour
{
    private CameraScale CameraScale;
    [Inject]
    public void Construct(CameraScale cameraScale)
    {
        CameraScale = cameraScale;
    }
    public float ZoneCameraScale;
    public Transform CenterZonePoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "player")
        {
            CameraScale.SetCurrCameraZone(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "player")
        {
            CameraScale.UnselectCurrCameraZone(this);
        }
    }
}
