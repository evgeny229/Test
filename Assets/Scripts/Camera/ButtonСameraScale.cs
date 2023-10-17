using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class ButtonСameraScale : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private CameraScale CameraScale;

    [Inject]
    public void Construct(CameraScale cameraScale)
    {
        CameraScale = cameraScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        CameraScale.SetBigScale(true);
        CameraScale.SetLevelScale();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CameraScale.SetBigScale(false);
        CameraScale.SetZoneScale();
    }
}
