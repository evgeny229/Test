using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Zenject;
using static System.Console;
using static Common.MethodWith;
public class RingsController : MonoBehaviour
{
    [SerializeField] private int RingsCount;

    private List<Ring> Rings;
    private Ring _currSelectedRing;
    private int[] _rings = new int[0];
    private int _buyingId;
    private string dataPath;
    private string savePath;
    private PlayerParticleSystems PlayerParticleSystems;
    private EventProvider EventProvider;
    private SpriteRenderer PlayerSprite;

    [Inject]
    public void Construct(PlayerParticleSystems playerParticleSystems,
        EventProvider eventProvider, SpriteRenderer playerSprite)
    {
        PlayerParticleSystems = playerParticleSystems;
        EventProvider = eventProvider;
        PlayerSprite = playerSprite;
    }
    private void Start()
    {
        GetAllRings();
        AddToEventProvider();
        SetDataPath();
        LoadItems();
        CreateValuesFirstTime();
        TryRefreshValueCount();
        SelectCurrentRing();
    }
    private void OnDestroy()
    {
        EventProvider.RingUpdate -= SetParameterOfRing;
    }
    public void SetNewRing(int id)
    {
        if (_rings[id] <= 0) return;
        OpenNewRing(id);
        UpdateSelectedRing(id);
        SaveItems();
    }
    public void BuyNewRing()
    {
        _rings[_buyingId] = 1;
        SetNewRing(_buyingId);
        SaveItems();
    }

    private void GetAllRings()
    {
        Rings = new List<Ring>();
        for (int i = 0;i< RingsCount; i++)
            Rings.Add(Resources.Load<Ring>($"ScriptableObject/Ring{i}"));
    }
    private void SetDataPath()
    {
        dataPath = Application.persistentDataPath + "/Save/Rings/";
        savePath = dataPath + "rings";
    }
    private void AddToEventProvider()
    {
        EventProvider.RingUpdate += SetParameterOfRing;
    }
    private void SelectCurrentRing()
    {
        for (int i = 0; i < _rings.Length; i++)
            if (_rings[i] == 2)
            {
                SetNewRing(i);
                break;
            }
    }
    private void CreateValuesFirstTime()
    {
        if (_rings.Length != 0) return;
        _rings = new int[RingsCount];
        _rings[0] = 2;
        SaveItems();
    }
    private void TryRefreshValueCount()
    {
        if (_rings.Length >= RingsCount) return;
        int[] a = new int[RingsCount];
        Array.Copy(_rings, a, _rings.Length);
        _rings = a;
    }
    private void LoadItems()
    {
        if (!Directory.Exists(dataPath))
            Directory.CreateDirectory(dataPath);
        if (File.Exists(savePath))
            using (FileStream fs = File.Open(savePath, FileMode.Open))
            {
                if (fs.Length > 0)
                {
                    object graphItems = new BinaryFormatter().Deserialize(fs);
                    if (graphItems != null)
                        _rings = (int[])graphItems;
                }
            }
    }
    private void SaveItems()
    {
        if (!Directory.Exists(dataPath))
            Directory.CreateDirectory(dataPath);
        using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate))
        {
            object g = (object)_rings;
            new BinaryFormatter().Serialize(fs, g);
        }

    }
    private void UpdateSelectedRing(int id)
    {
        for (int i = 0; i < _rings.Length; i++)
            if (_rings[i] == 2)
                _rings[i] = 1;
        _rings[id] = 2;
    }
    private void OpenNewRing(int ringId)
    {
        _currSelectedRing = Rings[ringId];
        EventProvider.RingUpdate.Invoke(_currSelectedRing);
    }
    private void SetParameterOfRing(Ring ring)
    {
        PlayerSprite.sprite = ring.Sprite;
        PlayerParticleSystems.SetColor(ring.Colors);
    }
}
