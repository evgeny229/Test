using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class StateMachine : IInitializable
{
    public bool FirstPlay { get; private set; }

    public void Initialize()
    {
        UpdateFirstPlay();
    }

    private void UpdateFirstPlay()
    {
        if (PlayerPrefs.GetInt("FirstPlay") == 0)
        {
            FirstPlay = true;
            PlayerPrefs.SetInt("FirstPlay", 1);
        }
    }
}
