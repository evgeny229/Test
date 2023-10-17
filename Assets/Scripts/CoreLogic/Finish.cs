using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Finish : PickableObject
{
    private EventProvider EventProvider;
    [Inject]
    public void Construct(EventProvider eventProvider)
    {
        EventProvider = eventProvider;
    }
    public override void Action(Collider2D collision) => EventProvider.GameComplete.Invoke();
}
