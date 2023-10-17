using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameState :IInitializable
{
    public bool GameIsPaused { get; set; }
    public bool ActiveCreateMode { get; set; }
    public bool IsSelectModeEnabled { get; set; }

    private CanvasA CanvasA;
    private PlayerDestroyedObject PlayerDestroyedObj;
    private PlayerParticleSystems PlayerParticleSystems;
    private CameraPosition CameraPosition;
    private LevelCreateController LevelCreateController;
    public GameState()
    {

    }
    [Inject]
    public GameState(CameraPosition cameraPosition, CanvasA canvasA, PlayerDestroyedObject playerDestroy, PlayerParticleSystems playerParticleSystems, LevelCreateController levelCreateController)
    {
        CameraPosition = cameraPosition;
        CanvasA = canvasA;
        PlayerDestroyedObj = playerDestroy;
        PlayerParticleSystems = playerParticleSystems;
        LevelCreateController = levelCreateController;
    }

    public void SetToLobby()
    {
        GameIsPaused = true;
    }
    public void SetToCreateLevel()
    {
        ActiveCreateMode = true;
        CameraPosition.transform.position = LevelCreateController.CreateLevelParent.position;
    }
    public void SetToGame()
    {
        ActiveCreateMode = false;
        CameraPosition.transform.position = Vector3.zero;
    }
    public void Initialize()
    {
        LoadGameFps();
    }
    private static void LoadGameFps()
    {
        int fpsCount = PlayerPrefs.GetInt("Fps");
        if (fpsCount == 0)
            fpsCount = 60;
        PlayerPrefs.SetInt("Fps", fpsCount);
        Application.targetFrameRate = fpsCount;
    }
}
