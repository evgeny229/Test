using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TraectoriaController : MonoBehaviour
{
    [SerializeField] private Transform ParentOfTraectoria;
    [SerializeField] private int countPoints;
    [SerializeField] private float kpTraekt;
    [SerializeField] private float dt;
    [SerializeField] private GameObject pointTraekt;

    private Traektor[] pointsTraeckt;
    private float traektTime;
    private int unselectedTraectoriaId;
    private float PowerOfJump;
    private UIInputController UIInputController;
    private RingJump RingJump;
    private EventProvider EventProvider;
    private GameObjectsFabric GameObjectsFabric;
    private Transform Player;
    [Inject]
    public void Construct(UIInputController uiInputController,
        RingJump ringJump,
        EventProvider eventProvider,
        GameObjectsFabric gameObjectsFabric,
        Transform player)
    {
        UIInputController = uiInputController;
        RingJump = ringJump;
        EventProvider = eventProvider;
        GameObjectsFabric = gameObjectsFabric;
        Player = player;
    }
    private void Start()
    {
        pointsTraeckt = new Traektor[countPoints];
        unselectedTraectoriaId = countPoints;
        pointsTraeckt = GameObjectsFabric.InstantiateTraectoriaPoints(pointTraekt, ParentOfTraectoria, countPoints);
        PowerOfJump = RingJump.powerOfOneJump;

        AddMethodsToEventProvider();
        //ActiveTraectoria(false);
    }
    private void Update()
    {
        UpdatePositionsOfTraectoria();
        UpdateActiveOfTraectoria();
    }
    private void OnDestroy()
    {
        RemoveMethodsFromEventProvider();
    }
    public void OnMouseButtonDown(Vector2 downPosition)
    {
        ActiveTraectoria(true);
    }
    public void OnMouseButtonUp()
    {
        ActiveTraectoria(false);
    }
    public void OnRingUpdate(Ring ring)
    {
        PowerOfJump = ring.PowerJump;
    }

    private void UpdateActiveOfTraectoria()
    {       
        FindFirstUnselectedId();
        SelectMaxPoints();
    }
    private void SelectMaxPoints()
    {
        for (int i = 0; i < unselectedTraectoriaId; i++)
            pointsTraeckt[i].ActiveTraectoriaImage(true);
        for (int j = unselectedTraectoriaId; j < countPoints; j++)
            pointsTraeckt[j].ActiveTraectoriaImage(false);
    }
    private void FindFirstUnselectedId()
    {
        unselectedTraectoriaId = countPoints;
        for (int i = 0; i < countPoints; i++)
            if (pointsTraeckt[i].IsTouchingLayers())
            {
                unselectedTraectoriaId = i;
                break;
            }
    }
    private void ActiveTraectoria(bool active)
    {
        ParentOfTraectoria.gameObject.SetActive(active);
    }
    private void UpdatePositionsOfTraectoria()
    {
        traektTime = dt;
        Vector2 NewVector = UIInputController.NewVector.normalized;
        float kpOfRange = RingJump.kpOfRange;
        //NewVector = new Vector2(1, 1).normalized;
        float deltaX = 0;
        float deltaY = 0;
        float deltaXOfOneDt = kpTraekt * kpOfRange * NewVector.x * dt * PowerOfJump;
        float deltaYOfOneDtForSpeed = kpTraekt * kpOfRange * NewVector.y * dt * PowerOfJump;
        for (int i = 0; i < countPoints; i++)
        {
            pointsTraeckt[i].transform.position = GetPositionOfTraektoria();
            traektTime+=dt;
        }
        Vector2 GetPositionOfTraektoria()
        {
            deltaX += deltaXOfOneDt;
            deltaY += deltaYOfOneDtForSpeed;
            return (Vector2)Player.position + new Vector2(deltaX, deltaY + GetDeltaYOfDtForAcceleration(traektTime));
        }
        float GetDeltaYOfDtForAcceleration(float dt)
        {
            return -RingJump.GravityScaleFly * Mathf.Pow(dt,2) / 2;
        }
    }
    private void AddMethodsToEventProvider()
    {
        EventProvider.MouseButtonDownUpdate += OnMouseButtonDown;
        EventProvider.MouseButtonUpUpdate += OnMouseButtonUp;
        EventProvider.RingUpdate += OnRingUpdate;
    }
    private void RemoveMethodsFromEventProvider()
    {
        EventProvider.MouseButtonDownUpdate -= OnMouseButtonDown;
        EventProvider.MouseButtonUpUpdate -= OnMouseButtonUp;
        EventProvider.RingUpdate -= OnRingUpdate;
    }
}
