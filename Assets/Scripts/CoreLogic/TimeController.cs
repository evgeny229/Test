using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TimeController : MonoBehaviour
{
    private float countFrames;
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
    }
    private void OnDestroy()
    {
        RemoveMethodsFromEventProvider();
    }
    public void OnMouseButtonDown(Vector2 currPos)
    {

    }
    public void OnMouseButtonUp()
    {
    }
    private void Update()
    {
        TryUpdateTimeScale();
    }
    public void SetSlowTime()
    {
        Time.timeScale = 0.001f;
    }

    private void TryUpdateTimeScale()
    {
        if (GameState.GameIsPaused) return;
        countFrames = (float)((float)1 / (float)Time.unscaledDeltaTime);
        float setTimeScale = 60f / countFrames;
        if (setTimeScale > 99.9f)
            setTimeScale = 99.9f;
        Time.timeScale = setTimeScale;
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
}
