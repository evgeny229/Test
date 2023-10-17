using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    AudioSource As;
    public void Start()
    {
        As = transform.GetComponent<AudioSource>();
    }
}
