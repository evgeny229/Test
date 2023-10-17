using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RingJump : MonoBehaviour
{
    [SerializeField] private float NeedTimetoUpJumpPower;
    [SerializeField] private float TimetoUpJumpPower;
    public float powerOfOneJump;
    public float GravityScaleFly;
    public float kpOfRange { get; set; }
    public int maxCountJumps;


    private int CountJumps;
    private float newJumpValueProgress;
    private float PowerZone;

    private Collider2D ColliderOfPlayer;
    private CameraInGameScene CameraInGameScene;
    private UIInputController UIInputController;
    private Rigidbody2D PlayerRigidbody2D;
    private MusicableObject MusicableObject;
    private EffectController EffectController;
    private EventProvider EventProvider;
    private GameConfig GameConfig;
    private MediatorCore MediatorCore;

    [Inject]
    public void Construct(CameraInGameScene cameraInGameScene,
        UIInputController uiInputController,
        Rigidbody2D playerRigidBody, MusicableObject musicableObject,
        EffectController effectController,
        EventProvider eventProvider, GameConfig gameConfig,
        MediatorCore mediatorCore)
    {
        CameraInGameScene = cameraInGameScene;
        UIInputController = uiInputController;
        PlayerRigidbody2D = playerRigidBody;
        MusicableObject = musicableObject;
        EffectController = effectController;
        EventProvider = eventProvider;
        GameConfig = gameConfig;
        MediatorCore = mediatorCore;
    }
    private void Start()
    {
        SetPowerZone();
        AddMethodsToEventProvider();
        ColliderOfPlayer = GetComponent<Collider2D>();
    }
    private void OnDestroy()
    {
        RemoveMethodsFromEventProvider();
    }
    public void Update()
    {
        Setkp();
        AddJumpProgress();
    }

    private void UpdateFlyState()
    {
        if (ColliderOfPlayer.IsTouchingLayers())
        {
            PlayerRigidbody2D.gravityScale = 0;
            SetZeroVelocity();
            MediatorCore.SetParticleSystem(PlayerFlyState.Stay);
        }
        else
        {
            PlayerRigidbody2D.gravityScale = GravityScaleFly;
            if(CountJumps>0)
                MediatorCore.SetParticleSystem(PlayerFlyState.FirstJump);
            else
                MediatorCore.SetParticleSystem(PlayerFlyState.LastJump);
        }
    }
    public void OnMouseButtonUp()
    {
        Jump(UIInputController.NewVector);
    }
    public void OnMouseButtonDown(Vector2 currPos)
    { 
    }
    public void Jump(Vector2 jumpVector)
    {
        if (CountJumps == 0 || jumpVector.magnitude < PowerZone * 0.1f) return;

        CountJumps--;
        SetZeroVelocity();
        //jumpVector = new Vector2(1, 1);
        PlayerRigidbody2D.AddForce((jumpVector.normalized) * powerOfOneJump * kpOfRange);
        if (EffectController.UseShaking)
            CameraInGameScene.CameraRigidbody2D.AddForce((jumpVector.normalized) * powerOfOneJump * kpOfRange * 0.75f);
        MusicableObject.PlayOneShot(1);
    }

    private void SetZeroVelocity()
    {
        PlayerRigidbody2D.velocity = Vector2.zero;
    }

    public void OnRingUpdate(Ring ring)
    {
        CountJumps = 0;
        powerOfOneJump = ring.PowerJump;
        maxCountJumps = ring.JumpCount;
    }

    private void AddJumpProgress()
    {
        if (CountJumps >= maxCountJumps || !ColliderOfPlayer.IsTouchingLayers()) return;
        newJumpValueProgress += Time.deltaTime;
        TryAddJump();
    }
    private void TryAddJump()
    {
        if(newJumpValueProgress > GameConfig.TimeToReloadJump)
        {
            newJumpValueProgress = 0;
            CountJumps++;
        }    
    }
    private void AddMethodsToEventProvider()
    {
        EventProvider.MouseButtonUpUpdate += OnMouseButtonUp;
        EventProvider.MouseButtonDownUpdate += OnMouseButtonDown;
        EventProvider.RingUpdate += OnRingUpdate;
    }
    private void RemoveMethodsFromEventProvider()
    {
        EventProvider.MouseButtonUpUpdate -= OnMouseButtonUp;
        EventProvider.MouseButtonDownUpdate -= OnMouseButtonDown;
        EventProvider.RingUpdate -= OnRingUpdate;
    }
    private void Setkp()
    {
        kpOfRange = Mathf.Clamp(UIInputController.NewVector.magnitude / PowerZone, 0.5f, 1f);
    }
    private void SetPowerZone()
    {
        PowerZone = Screen.resolutions[0].width / 2.5f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateFlyState();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        UpdateFlyState();
    }
}
