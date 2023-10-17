using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Ring", menuName ="Rings/Info")]
public class Ring : ScriptableObject
{
    [SerializeField] private int _id;
    public int Id => this._id;

    [SerializeField] private Color[] _colors;
    public Color[] Colors => _colors;

    [SerializeField] private Sprite CurrSprite;
    public Sprite Sprite => CurrSprite;

    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private RingTypeAttack _typeOfAttack;
    public RingTypeAttack TypeOfAttack => _typeOfAttack;

    [SerializeField] private int _healthCount;
    public int HealthCount => _healthCount;

    [SerializeField] private float _powerJump;
    public float PowerJump => _powerJump;

    [SerializeField] private int _jumpCount;
    public int JumpCount => _jumpCount;

    [SerializeField] private float _scale;
    public float Scale => _scale;

    [SerializeField] private RingRarity _rarity;
    public RingRarity Rarity => _rarity;
}