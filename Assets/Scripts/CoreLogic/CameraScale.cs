using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class CameraScale : MonoBehaviour
{
    [SerializeField] private float kpScaleCamera;

    public bool SetBigCamScale { get; private set; }
    public float needCameraScale { get; private set; }
    private float zoneCameraScale;
    private float levelScale;
    public CameraZone CameraZone;
    private Camera CameraA;
    private EventProvider EventProvider;
    private List<CameraZone> currCameraZones;
    private CanvasA CanvasA;
    private DestroyedObj PlayerDestroyedObj;
    private CameraPosition CameraPosition;
    private GameState GameState;
    private Transform Player;
    [Inject]
    public void Construct(Camera camera, EventProvider eventProvider, CanvasA canvasA,
        DestroyedObj playerDestroyedObj, CameraPosition cameraPosition,
        GameState gameState, Transform player)
    {
        CameraA = camera;
        EventProvider = eventProvider;
        CanvasA = canvasA;
        PlayerDestroyedObj = playerDestroyedObj;
        CameraPosition = cameraPosition;
        GameState = gameState;
        Player = player;
    }
    private void Start()
    {
        AddMethodsToEventProvider();
        currCameraZones = new List<CameraZone>();
    }

    private void Update()
    {
        CameraScaleWhenGameIsntPaused();
        CameraScaleAfterDeath();
        CameraScaleInCreatorMod();
    }
    private void OnDestroy()
    {
        RemoveMethodsFromEventProvider();
    }



    public void SetBigScale(bool bigScale)
    {
        SetBigCamScale = bigScale;
    }
    public void SetZoneScale()
    {
        needCameraScale = zoneCameraScale;
    }
    public void SetLevelScale()
    {
        needCameraScale = levelScale;
    }
    public void SetCurrCameraZone(CameraZone CamZone)
    {
        EventProvider.SetNewCameraZoneUpdate.Invoke(CamZone);
        CameraZone = CamZone;
        zoneCameraScale = CamZone.ZoneCameraScale;
        if (!SetBigCamScale)
            SetZoneScale();
        if (!currCameraZones.Contains(CamZone))
            currCameraZones.Add(CamZone);
    }
    public void UnselectCurrCameraZone(CameraZone camZone)
    {
        currCameraZones.Remove(camZone);
        TrySetCameraZone();
    }

    private void AddMethodsToEventProvider()
    {
        EventProvider.LevelScaleUpdate += OnLevelScaleUpdate;
    }
    private void RemoveMethodsFromEventProvider()
    {
        EventProvider.LevelScaleUpdate -= OnLevelScaleUpdate;
    }
    private void CameraScaleWhenGameIsntPaused()
    {
        if (!GameState.GameIsPaused && !GameState.ActiveCreateMode)
        {
            CameraA.orthographicSize = Mathf.Lerp(CameraA.orthographicSize, needCameraScale, 2f * Time.unscaledDeltaTime);
            transform.localScale = new Vector3(CameraA.orthographicSize, CameraA.orthographicSize, 1);
        }
    }
    private void CameraScaleInCreatorMod()
    {
        if (GameState.ActiveCreateMode)
        {
            CameraA.orthographicSize = Mathf.Lerp(CameraA.orthographicSize, needCameraScale, 2f * Time.unscaledDeltaTime);
            kpScaleCamera = Mathf.Lerp(kpScaleCamera, 0, Time.unscaledDeltaTime * 0.1f);
            if (CanvasA.activeScaleMinusCamera)
                SetCameraScaleInCreatorMod(5f);
            if (CanvasA.activeScalePlusCamera)
                SetCameraScaleInCreatorMod(25f);
        }
    }
    private void CameraScaleAfterDeath()
    {
        if (PlayerDestroyedObj.IsDead && GameState.GameIsPaused)
        {
            CameraA.orthographicSize = Mathf.Lerp(CameraA.orthographicSize, -0.1f, 7f * Time.unscaledDeltaTime);
            if (CameraA.orthographicSize < 0)
            {
                PlayerDestroyedObj.IsDead = false;
                CameraA.transform.position = Player.position;
            }
        }
    }
    private void SetCameraScaleInCreatorMod(float b)
    {
        kpScaleCamera = Mathf.Lerp(kpScaleCamera, 1, Time.unscaledDeltaTime * 2f);
        needCameraScale = Mathf.Lerp(needCameraScale, b, Time.unscaledDeltaTime * 2 * kpScaleCamera);
        CameraPosition.changeCameraTransform = false;
    }
    private void OnLevelScaleUpdate(float newLevelScale)
    {
        levelScale = newLevelScale;
    }
    private void TrySetCameraZone()
    {
        if (currCameraZones.Count > 0)
            if (currCameraZones[0] != null)
                SetCurrCameraZone(currCameraZones[0]);
    }
}
