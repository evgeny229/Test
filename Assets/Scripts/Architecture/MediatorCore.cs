using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject; 
public class MediatorCore : MonoBehaviour
{
    private PlayerParticleSystems PlayerParticleSystems;
    private CameraInGameScene CameraInGameScene;
    [Inject]
    public void Construct(PlayerParticleSystems playerParticleSystems,
        CameraInGameScene cameraInGameScene)
    {
        PlayerParticleSystems = playerParticleSystems;
        CameraInGameScene = cameraInGameScene;
    }

    public void SetParticleSystem(PlayerFlyState playerFlyState)
    {
        PlayerParticleSystems.SetParticleSystem(playerFlyState);
    }
    public void AddCameraForce(Vector2 force)
    {
        CameraInGameScene.AddForce(force);
    }
}
