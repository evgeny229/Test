using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraPosition : MonoBehaviour
{
    public CameraZone CameraZone;
    public Vector2 startMovedPosition;
    public Vector2 startPointerPosition;
    public Vector2 LastPosition;
    public bool changeCameraTransform;

    private EventProvider EventProvider;
    private CameraScale CameraScale;
    private LevelCreateController LevelCreateController;
    private Transform Player;
    private GameState GameState;
    [Inject]
    public void Construct(EventProvider eventProvider,
        CameraScale cameraScale, LevelCreateController levelCreateController, 
        GameState gameState, Transform player)
    {
        EventProvider = eventProvider;
        CameraScale = cameraScale;
        LevelCreateController = levelCreateController;
        GameState = gameState;
        Player = player;
    }
    private void Start()
    {
        EventProvider.SetNewCameraZoneUpdate += OnCameraZoneUpdate;
    }
    private void Update()
    {
        if (GameState.ActiveCreateMode) return;
        if (GameState.GameIsPaused)
            MoveToPlayer();
        else
        {
            TryMoveToCameraZone();
            TryMoveToCenter();
        }
    }
    private void OnDestroy()
    {
        EventProvider.SetNewCameraZoneUpdate -= OnCameraZoneUpdate;
    }


    private void MoveToPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, Player.position, 7f * Time.unscaledDeltaTime);
    }
    private void OnCameraZoneUpdate(CameraZone CamZone)
    {
        CameraZone = CamZone;
    }
    private void TryMoveToCenter()
    {
        if (CameraScale.SetBigCamScale == true && LevelCreateController.CurrLevel != null)
            transform.position = Vector3.Lerp(transform.position, LevelCreateController.CurrLevel.transform.position, 3f * Time.unscaledDeltaTime);
    }
    private void TryMoveToCameraZone()
    {
        if (CameraZone != null && CameraScale.SetBigCamScale == false)
            transform.position = Vector3.Lerp(transform.position, CameraZone.CenterZonePoint.transform.position, 3f * Time.unscaledDeltaTime);
    }
}
