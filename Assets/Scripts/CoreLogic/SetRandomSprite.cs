using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Color[] colors;
    private SpriteRenderer SpriteRenderer;
    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length>0)
            SpriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public void SetColor(int difficulty)//0-base 1-ice 2-sand
    {
        if(difficulty<colors.Length)
            SpriteRenderer.color = colors[difficulty];
    }
}
