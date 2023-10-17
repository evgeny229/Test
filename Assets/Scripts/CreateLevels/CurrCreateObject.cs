using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CurrCreateObject : MonoBehaviour
{

    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Bricks;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> BackGrounds;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Guns;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Deathes;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Zones;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Mechanisms;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Decor;
    /// <summary> Bricks and collided sprites</summary>
    public List<GameObject> Other;

    public List<GameObject> Objects2;
    CanvasA canv;
    /// <summary> id of currSelect obj in List </summary>
    public int CurrObject = 0;
    /// <summary> id of curr List 0-bricks 1-backgrounds 2-guns 3-deathes 4-zoines 5-mexanisms 6- decor 7-prizes 8-other </summary>
    public int CurrList = 0;
    const int ListCount = 8;

    public Transform level;
    //private CanvasA CanvasA;
    private LevelCreateController LevelCreateController;
    [Inject]
    public void Construct(LevelCreateController levelCreateController)
    {
        LevelCreateController = levelCreateController;
    }
    private void Start()
    {
    }
    public void Update()
    {       
    }
    public void IncreaceList()
    {
        CurrList++;
        if (CurrList == ListCount)
        {
            CurrList = 0;
        }
        CurrObject = 0; 
        UpdateSelectObjectImage();
    }
    public void DecreateList()
    {
        CurrList--;
        if (CurrList ==-1)
        {
            CurrList = ListCount - 1;
        }
        CurrObject = 0;
        UpdateSelectObjectImage();
    }
    public void IncreaseId()
    {
        CurrObject++;
        if (CurrObject >= CurrListCount())
            CurrObject = 0;
        UpdateSelectObjectImage();
    }
    public void DecreaseId()
    {
        CurrObject--;
        if (CurrObject == -1)
            CurrObject = CurrListCount()-1;
        UpdateSelectObjectImage();
    }
    int CurrListCount()
    {
        return CurrentSelectedList().Count;
    }
    List<GameObject> CurrentSelectedList()
    {
        switch (CurrList)
        {
            case 0:
                return Bricks;
            case 1:
                return BackGrounds;
            case 2:
                return Guns;
            case 3:
                return Deathes;
            case 4:
                return Zones;
            case 5:
                return Mechanisms;
            case 6:
                return Decor;
            case 7:
                return Other;
        }
        return null;
    }

    public void UpdateSelectObjectImage()
    {
        List<GameObject> currSelectedList = CurrentSelectedList();
        if (currSelectedList[CurrObject].transform.GetComponent<SpriteRenderer>() != null)
        {
            Func(currSelectedList[CurrObject].transform.GetComponent<SpriteRenderer>());

        }
        else if (currSelectedList[CurrObject].transform.childCount > 0)
        {
            for (int i = 0; i < currSelectedList[CurrObject].transform.childCount; i++)
            {
                if (currSelectedList[CurrObject].transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
                {
                    Func(currSelectedList[CurrObject].transform.GetChild(0).GetComponent<SpriteRenderer>());
                    break;
                }
            }
        }
        void Func(SpriteRenderer sp)
        {
            Image im = transform.GetComponent<Image>();
            im.sprite = sp.sprite;
            im.color = sp.color;
            im.SetNativeSize();
            im.GetComponent<RectTransform>().sizeDelta = im.GetComponent<RectTransform>().sizeDelta.normalized * 335;
        }
    }
    public void CreateCurrObject()
    {
        CreateCurrObject2();
    }
    public GameObject CreateCurrObject2()
    {
        GameObject g = null;
        List<GameObject> currSelectedList = CurrentSelectedList();
        if (CurrObject > -1)
        {
            g = GameObject.Instantiate(currSelectedList[CurrObject], LevelCreateController.CreateLevelParent);
            g.transform.position = LevelCreateController.CreateLevelParent.position;
        }
        else
        {
            Level lev = LevelCreateController.CreateLevelParent.GetComponent<Level>();
            if (CurrObject == -1)//spawn last point
            {
                g = GameObject.Instantiate(Objects2[0], LevelCreateController.CreateLevelParent);
                lev.lastPoint = g.transform;
            }
            else if(CurrObject == -2)//center
            {
                g = GameObject.Instantiate(Objects2[1], LevelCreateController.CreateLevelParent);
                lev.Center = g.transform;
            }
            else if(CurrObject == -10)//spawnPoint
            {
                g = GameObject.Instantiate(Objects2[2], LevelCreateController.CreateLevelParent);
            }
        }
        g.GetComponent<MovablePartLevel>().OnSpawn(CurrObject, CurrList);
        return g;
    }

    public void DeleteCurrObject()
    {
        if(canv.createLevelSelectedObject != null)
        {
            Destroy(canv.createLevelSelectedObject.gameObject);
            canv.UnselectSelObject();
        }
    }
    public void CopyCurrObject()
    {
        if (canv.createLevelSelectedObject != null)
        {
            GameObject g = GameObject.Instantiate(canv.createLevelSelectedObject.gameObject);
            g.transform.position = canv.createLevelSelectedObject.transform.position;
            g.transform.SetParent(canv.createLevelSelectedObject.transform.parent);
            g.GetComponent<MovablePartLevel>().RandomizeIndex();
        }
    }
}
