using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventProvider
{
    public Action<Ring> RingUpdate;
    public Action<float> LevelScaleUpdate;
    public Action<CameraZone> SetNewCameraZoneUpdate;
    public Action<Vector2> MouseButtonDownUpdate;
    public Action MouseButtonUpUpdate;
    public Action PlayerIsDead;
    public Action GameComplete;
}
