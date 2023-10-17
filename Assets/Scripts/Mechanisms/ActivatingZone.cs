using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingZone : ActivatingBaseObject
{
    public void OnPointerDown()
    {
        if (GameState.ActiveCreateMode) return;
        Active();
    }

    public void OnPointerUp()
    {
        if (GameState.ActiveCreateMode) return;        
        Deactive();
    }
}
