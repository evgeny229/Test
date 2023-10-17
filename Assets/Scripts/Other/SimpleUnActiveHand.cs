using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUnActiveHand : MonoBehaviour
{
    private Image imageHand;
    private void Start()
    {
        imageHand = GetComponent<Image>();
    }
    public void UnActiveImage()
    {
        imageHand.enabled = false;
    }
}
