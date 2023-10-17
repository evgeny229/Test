using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System;

public class UIInputController : MonoBehaviour
{
    [SerializeField] private RectTransform StaticObj;
    [SerializeField] private RectTransform DynamicObj;
    [SerializeField] private bool UseTouches;
    public Vector2 NewVector { get; set; }

    private RectTransform RectCanvas;
    private Vector2 CurrPos 
    {
        get => currPos; 
        set
        {
            UpdateDynamicObject();
            currPos = value;
        } 
    }
    private Vector2 currPos;
    private Animator Hand;
    private EventProvider EventProvider;
    private GameState GameState;
    [Inject]
    public void Construct(EventProvider eventProvider, GameState gameState)
    {
        EventProvider = eventProvider;
        GameState = gameState;
    }
    private void Start()
    {
        AddMethodsToEventProvider();

        RectCanvas = GetComponent<RectTransform>();
        Hand = DynamicObj.GetChild(0).GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        RemoveMethodsFromEventProvider();
    }
    public void Update()
    {
        if (UseTouches)
            UpdateTouchStateTouchpad();
        else
            UpdateTouchStateMouse();
        TryUpdateNewVector();
    }
    public void OnMouseButtonDown(Vector2 downPosition)
    {
        SetPositionStaticObject();
        ActiveStaticAndDinamycObjects();
        TryPlayHandAnimation();
    }
    public void OnMouseButtonUp()
    {
        TryPlayReversedHandAnimation();
        DeactiveStaticAndDynamicObjects();
    }


    private void AddMethodsToEventProvider()
    {
        EventProvider.MouseButtonDownUpdate += OnMouseButtonDown;
        EventProvider.MouseButtonUpUpdate += OnMouseButtonUp;
    }
    private void RemoveMethodsFromEventProvider()
    {
        EventProvider.MouseButtonDownUpdate -= OnMouseButtonDown;
        EventProvider.MouseButtonUpUpdate -= OnMouseButtonUp;
    }
    private void DeactiveStaticAndDynamicObjects()
    {
        StaticObj.gameObject.SetActive(false);
        DynamicObj.gameObject.SetActive(false);
    }
    private void TryUpdateNewVector()
    {
        if (GameState.GameIsPaused || GameState.ActiveCreateMode) return;      
        UpdateNewVector();
    }
    private void UpdateTouchStateMouse()
    {
        CurrPos = Input.mousePosition;
        TryInvokeUpDownButtonMethods(Input.GetMouseButtonUp(0), Input.GetMouseButtonDown(0));
    }
    private void UpdateTouchStateTouchpad()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            CurrPos = touch.position;
            TryInvokeUpDownButtonMethods(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled, touch.phase == TouchPhase.Began);
        }
    }
    private void TryInvokeUpDownButtonMethods(bool isUp, bool isDown)
    {
        if (isDown)
            EventProvider.MouseButtonDownUpdate.Invoke(CurrPos);
        if (isUp)
            EventProvider.MouseButtonUpUpdate.Invoke();
    }
    private void UpdateNewVector()
    {
        NewVector = StaticObj.anchoredPosition - DynamicObj.anchoredPosition;
    }
    private void UpdateDynamicObject()
    {
        DynamicObj.anchoredPosition = CurrPos;
    }
    private void TryPlayHandAnimation()
    {        
        if (Hand.gameObject.activeSelf)
        {
            Hand.GetComponent<Image>().enabled = true;
            Hand.Rebind();
        }
    }
    private void ActiveStaticAndDinamycObjects()
    {
        StaticObj.gameObject.SetActive(true);
        DynamicObj.gameObject.SetActive(true);
    }
    private void SetPositionStaticObject()
    {
        StaticObj.anchoredPosition = CurrPos / RectCanvas.localScale;
    }
    private void TryPlayReversedHandAnimation()
    {
        if (!Hand.gameObject.activeSelf)
            DynamicObj.gameObject.SetActive(false);
        else
            Hand.Play("HandAnimReverced");
    }
}
