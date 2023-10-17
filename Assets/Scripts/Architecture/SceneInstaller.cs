using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private CameraInGameScene CameraInGameScene;
    [SerializeField] private LevelCreateController LevelCreateController;
    [SerializeField] private CanvasA CanvasA;
    [SerializeField] private SpriteRenderer PlayerSprite;
    [SerializeField] private RingsController AllRings;
    [SerializeField] private AudioController CurrAudioController;
    [SerializeField] private TimeController CurrTimeController;
    [SerializeField] private CurrCreateObject CurrCreateObject;
    [SerializeField] private PanelSelectRing CurrPanelSelectRing;
    [SerializeField] private PlayerDestroyedObject PlayerDestroyedObj;
    [SerializeField] private MusicableObject PlayerMusicableObject;
    [SerializeField] private PlayerParticleSystems PlayerParticleSystems;
    [SerializeField] private EffectController EffectController;
    [SerializeField] private Camera Camera;
    [SerializeField] private Rigidbody2D PlayerRigidBody;
    [SerializeField] private RingJump RingJump;
    [SerializeField] private UIInputController UIInputController;
    [SerializeField] private ScoreController ScoreController;
    [SerializeField] private CameraPosition CameraPosition;
    [SerializeField] private CameraScale CameraScale;
    [SerializeField] private TraectoriaController TraectoriaController;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private GameObjectsFabric GameObjectsFabric;
    [SerializeField] private GameConfig GameConfig;
    [SerializeField] private MediatorCore MediatorCore;
    public override void InstallBindings()
    {
        BindReferenceComponent(CameraInGameScene);
        BindReferenceComponent(LevelCreateController);
        BindReferenceComponent(CanvasA);
        BindReferenceComponent(PlayerSprite);
        BindReferenceComponent(CurrAudioController);
        BindReferenceComponent(CurrTimeController);
        BindReferenceComponent(CurrCreateObject);
        BindReferenceComponent(CurrPanelSelectRing);
        BindReferenceComponent(PlayerDestroyedObj);
        BindReferenceComponent(PlayerMusicableObject);
        BindReferenceComponent(AllRings);
        BindReferenceComponent(PlayerParticleSystems);
        BindReferenceComponent(Camera);
        BindReferenceComponent(PlayerRigidBody);
        BindReferenceComponent(EffectController);
        BindReferenceComponent(RingJump);
        BindReferenceComponent(UIInputController);
        BindReferenceComponent(ScoreController);
        BindReferenceComponent(CameraPosition);
        BindReferenceComponent(CameraScale);
        BindReferenceComponent(TraectoriaController);
        BindReferenceComponent(PlayerTransform);
        BindReferenceComponent(GameObjectsFabric);
        BindReferenceComponent(GameConfig);
        BindReferenceComponent(MediatorCore);

        GameState gameState = new GameState();
        BindFromInstanceTransient(gameState);
        Container.BindInterfacesTo<GameState>().AsTransient();

        StateMachine stateMachine = new StateMachine();
        BindFromInstanceTransient(stateMachine);
        Container.BindInterfacesTo<StateMachine>().AsSingle(); 

        EventProvider eventProvider = new EventProvider();
        BindFromInstanceTransient(eventProvider);
        Container.BindInterfacesTo<EventProvider>().AsSingle(); 
    }

    private void BindFromInstanceTransient<T>(T reference)
    {
        Container
            .Bind<T>()
            .FromInstance(reference)
            .AsTransient();
    }

    private void BindRefenceClass<T>()
    {
        Container.Bind<T>().AsSingle();
    }

    private void BindReferenceComponent<T>(T Reference)
    {
        Container
            .Bind<T>()
            .FromInstance(Reference)
            .AsSingle();
    }
}
