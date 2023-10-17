using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelCreateController : MonoBehaviour
{    
    public Level CurrLevel;
    public Transform CreateLevelParent;
    public bool testLevel;

    private CameraScale CameraScale;
    private EventProvider EventProvider;
    private Transform Player;
    private CameraPosition CameraPosition;
    [Inject]
    public void Construct(EventProvider eventProvider, CameraScale  cameraScale,
        Transform player, CameraPosition cameraPosition)
    {
        EventProvider = eventProvider;
        CameraScale = cameraScale;
        Player = player;
        CameraPosition = cameraPosition;
    }
    public void AddLevel(GameObject gi = null)
    {
    }
    public void TestPlayLevel(GameObject levelPart)
    {
        AddLevel(levelPart);
        CameraPosition.transform.position = Vector3.zero;
    }
}
