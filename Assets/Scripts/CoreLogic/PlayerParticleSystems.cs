using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleSystems : MonoBehaviour
{
    public Color[] CurrColors;
    public ParticleSystem[] particleSystems;
    private PlayerFlyState lastPlayerFlyState;
    public void SetColor(Color[] colors)
    {
        CurrColors = colors;
        particleSystems[0].startColor = CurrColors[0];
    }
    public void SetParticleSystem(PlayerFlyState playerFlyState)
    {
        if (playerFlyState == lastPlayerFlyState) return;
        for (int i = 0; i < particleSystems.Length; i++)
            particleSystems[i].Stop();
        switch (playerFlyState)
        {
            case PlayerFlyState.Stay:
                particleSystems[0].Play();
                break;
            case PlayerFlyState.FirstJump:
                particleSystems[1].startColor = CurrColors[1];
                particleSystems[1].Play();
                break;
            case PlayerFlyState.LastJump:
                particleSystems[1].startColor = CurrColors[2];
                particleSystems[1].Play();
                break;
        }
        lastPlayerFlyState = playerFlyState;
    }
}
