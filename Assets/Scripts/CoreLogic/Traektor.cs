using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Traektor : MonoBehaviour
{
    private GameObject SpriteOfTraektoria;
    private Collider2D Collider2D;
    private void Awake()
    {
        SpriteOfTraektoria = transform.GetChild(0).gameObject;
        Collider2D = GetComponent<Collider2D>();
    }
    public bool IsTouchingLayers()
    {
        return Collider2D.IsTouchingLayers();
    }
    public void ActiveTraectoriaImage(bool active)
    {
        SpriteOfTraektoria.SetActive(active);
    }
}
