using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using Zenject;

[Serializable]
public class MovablePartLevel : MonoBehaviour
{
    /// <summary> -10 - spawnPoint </summary>
    public int ObjId;
    /// <summary> id of list to spawn </summary>
    public int ListId;

    public int IndexObj;
    public float PosX, PosY, RotZ, ScaleX, ScaleY;
    public float[] OtherParameters;
    /// <summary>random value </summary>
    public float mainParams;
    /// <summary>0-parent 1-wall,OtherIndex:/// </summary>
    public int[] MainIndex;
    /// <summary>///type:5 0-OtherTel,type:6 UsingObj,1-RotPoint,3- FinishPos, </summary>
    public int[] OtherIndex;
    ///<summary>тип двиг обьекта 1-brick 2fakel 3death 4gun 5teleport 6button7zone 8 cam zone 9 moving platform 10-center 11-enemy 12-spawn point</summary>
    public int type;
    ///<summary>тип пушки 0-normal 1-tnt //or using obj0  have other obj 1 have 2oth obj 2 3-timer</summary>
    public int typeGun;
    [NonSerialized]
    ///<summary>а двилается ли</summary>
    public bool isDragging = false;
    ///<summary>начальная позиция</summary>
    public Vector2 startPos;
    ///<summary>начальная позиция поинтера</summary>
    public Vector2 firstPosObject;
    ///<summary>коллайдер для удаления</summary>
    public Collider2D colliderCurrObject;
    CameraPosition CameraPosition;
    Camera CameraA;
    public bool nonDeletable;
    GameState GameState;
    CanvasA CanvasA;
    [Inject]
    public void Construct(CameraPosition cameraPosition, Camera camera,
        GameState gameState, CanvasA canvasA)
    {
        CameraPosition = cameraPosition;
        CameraA = camera;
        GameState = gameState;
        CanvasA = canvasA;
    }
    ///<summary>начало драга</summary>
    public void BBBBBBBBBBBBBBBBBBBBBBBBBBBB()
    {
        if (!GameState.IsSelectModeEnabled)
        {
            isDragging = true;
            CameraPosition.changeCameraTransform = false;
            //ObjectsOnScene.a.currCanvas.UnselectAllCreateLevelObjects();
            //lastSelectedObject = true;
            CanvasA.ActiveAnimatorKey(CanvasA.buttonsChangeTransform, true);
            if (CanvasA.createLevelSelectedObject != this)
            {
                CanvasA.SelectSelObject(this);
                switch (type)
                {
                    case 1://bricks
                        CanvasA.SelectCreateLevelObjectControllers(0);
                        break;
                    case 2://fakels
                        CanvasA.SelectCreateLevelObjectControllers(1);
                        CanvasA.allPartControllers[0].SetParameters("range", transform.GetChild(1).GetComponent<Light2D>().pointLightOuterRadius, 30f, 1);
                        break;
                    case 3://death
                        CanvasA.SelectCreateLevelObjectControllers(2);
                        CanvasA.allPartControllers[0].SetParameters("damage", GetComponent<Trap>()._damage, 10f, 0);
                        CanvasA.allPartControllers[1].SetParameters("health", GetComponent<DestroyedObj>().health, 100f, 0);
                        break;
                    case 4://gun
                        CanvasA.SelectCreateLevelObjectControllers(6);
                        Gun g = GetComponent<Gun>();
                        CanvasA.allPartControllers[0].SetParameters("time", g._time, 60f, 1);
                        float l = 0f;
                        if (g._toPlayer)
                            l = 1f;
                        CanvasA.allPartControllers[1].SetParameters("to player", l, 1f, 2);
                        //CanvasA.allPartControllers[2].SetParameters("power", g._power, 50f, 1);
                        //if (g.unselectVelocity)
                         //   l = 1f;
                        //else l = 0f;
                        //CanvasA.allPartControllers[3].SetParameters("unselect velocity", l, 1f, 2);
                        //if (g.onlyOneAmmo)
                        //    l = 1f;
                        //else l = 0f;
                        //CanvasA.allPartControllers[4].SetParameters("only one ammo", l, 1f, 2);
                        if (typeGun == 1)
                        {
                            //if (g.Ammo == AllUsedComponents.GameObjs[5])
                            //    l = 1f;
                            //else l = 0f;
                            CanvasA.allPartControllers[5].SetParameters("use bochka", l, 1f, 2);
                        }
                        else
                            CanvasA.allPartControllers[5].SetParameters("start delay", g._stayTime, 10f, 1);
                        break;
                    case 5://teleport
                        CanvasA.SelectCreateLevelObjectControllers(1);//
                        CanvasA.allPartControllers[0].SetParameters("select teleport", 0f, 1f, 3);
                        if (GetComponent<Teleport>().SecTeleport != null)
                        {
                            CanvasA.allPartControllers[0].textValueParameter.text = GetComponent<Teleport>().SecTeleport.gameObject.name;
                        }
                        break;
                    case 6://buttono
                        if (typeGun == 0)
                            CanvasA.SelectCreateLevelObjectControllers(2);
                        else if (typeGun == 1)
                            CanvasA.SelectCreateLevelObjectControllers(3);
                        else if (typeGun == 2)
                            CanvasA.SelectCreateLevelObjectControllers(4);
                        else if (typeGun == 3)
                            CanvasA.SelectCreateLevelObjectControllers(5);
                        CanvasA.allPartControllers[0].SetParameters("using obj", 0f, 1f, 3);//
                        if (GetComponent<ActivatingBaseObject>().FirstUsedObject != null)
                            CanvasA.allPartControllers[0].textValueParameter.text = GetComponent<ActivatingBaseObject>().FirstUsedObject.gameObject.name;
                        CanvasA.allPartControllers[1].SetParameters("speed", GetComponent<ActivatingBaseObject>().speed, 60f, 1);
                        if (typeGun > 0)
                        {
                            CanvasA.allPartControllers[2].SetParameters("other obj", 0f, 1f, 3);//
                            if (GetComponent<ActivatingBaseObject>().SecondUsedObject != null)
                                CanvasA.allPartControllers[2].textValueParameter.text = GetComponent<ActivatingBaseObject>().SecondUsedObject.gameObject.name;
                        }
                        if (typeGun == 3)//timer actionType
                            CanvasA.allPartControllers[4].SetParameters("act type", (float)GetComponent<ActivatingBaseObject>().ActionType, 6.5f, 0);//
                        if (typeGun == 3 || typeGun == 2)
                        {
                            CanvasA.allPartControllers[3].SetParameters("last point", 0f, 1f, 3);//
                            if (GetComponent<ActivatingBaseObject>().ThirdUsingObject != null)
                                CanvasA.allPartControllers[3].textValueParameter.text = GetComponent<ActivatingBaseObject>().ThirdUsingObject.gameObject.name;
                        }
                        break;
                    case 7://zone
                        CanvasA.SelectCreateLevelObjectControllers(1);
                        CanvasA.allPartControllers[0].SetParameters("power", GetComponent<Zone>().power, 10f, 1);
                        break;
                    case 8://camZone
                        CanvasA.SelectCreateLevelObjectControllers(1);
                        CanvasA.allPartControllers[0].SetParameters("screen size", GetComponent<CameraZone>().ZoneCameraScale, 20f, 1);
                        break;
                    case 9://MovingPlatform
                        //CanvasA.SelectCreateLevelObjectControllers(2);
                        //CanvasA.allPartControllers[0].SetParameters("range", GetComponent<MovingPlatform>().Delta, 10f, 1);
                        //CanvasA.allPartControllers[1].SetParameters("speed", GetComponent<MovingPlatform>().Speed, 15f, 1);
                        break;
                    case 10:
                        //CanvasA.SelectCreateLevelObjectControllers(3);
                        //CanvasA.allPartControllers[0].SetParameters("camera size", CanvasA.Level.GetComponent<Level>().CameraScale, 22f, 1);
                        //CanvasA.allPartControllers[1].SetParameters("range", GetComponent<MovingPlatform>().Delta, 10f, 1);
                        //CanvasA.allPartControllers[2].SetParameters("speed", GetComponent<MovingPlatform>().Speed, 15f, 1);
                        break;
                    case 11:
                        CanvasA.SelectCreateLevelObjectControllers(2);
                        CanvasA.allPartControllers[0].SetParameters("health", GetComponent<DestroyedObj>().health, 100f, 0);
                        CanvasA.allPartControllers[1].SetParameters("range", GetComponent<EnemyRing>()._needRange, 15f, 1);
                        break;
                }
                CanvasA.selectParentController.textValueParameter.text = transform.parent.gameObject.name;
            }
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                startPos = new Vector2(t.position.x, t.position.y);
            }
            else
                startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            firstPosObject = new Vector2(transform.position.x, transform.position.y);
        }
    }
    ///<summary>конец драга</summary>
    public void EEEEEEEEEEEEEEEEEEEEEEEEEEEE()
    {
        if (!CanvasA.isSelectModeEnabled)
        {
            isDragging = false;
            //ObjectsOnScene.a.currCanvas.createLevelSelectedObject = null;
        }
    }
    ///<summary>клик то есть выделить обьект</summary>
    public void PointerClickKKKKKKKKKKKKKKK()
    {
        if (CanvasA.isSelectModeEnabled)
        {
            if (CanvasA.selectedController.id < 6)
            {
                ///выбрать обект  
                if (CanvasA.createLevelSelectedObject.SetObject(gameObject))
                {
                    CanvasA.isSelectModeEnabled = false;
                    CanvasA.selectedController.textValueParameter.text = gameObject.name;
                }
            }
            else if (CanvasA.selectedController.id == 6)
            {
                if (CanvasA.createLevelSelectedObject.transform != transform)
                {
                    CanvasA.createLevelSelectedObject.transform.SetParent(transform);
                    CanvasA.isSelectModeEnabled = false;
                    CanvasA.selectedController.textValueParameter.text = transform.name;
                }
            }
            else if (CanvasA.selectedController.id == 8)
            {
                if (CanvasA.createLevelSelectedObject.SetObject(gameObject))
                {
                    CanvasA.isSelectModeEnabled = false;
                }
            }
        }
    }
    ///<summary>по нвжвтию на левую кнопку</summary>
    public void SelectNullRef(int id)
    {
        if (type == 5)
        {
            GetComponent<Teleport>().SecTeleport = null;
        }
        else if (type == 6)
        {
            if (id == 0)// using obj
            {
                GetComponent<ActivatingBaseObject>().FirstUsedObject = null;
            }
            else if (id == 2)
            {
                GetComponent<ActivatingBaseObject>().SecondUsedObject = null;
            }
            else if (id == 3)
            {
                GetComponent<ActivatingBaseObject>().ThirdUsingObject = null;
            }
        }
    }
    ///<summary>установить значение к текущему обьекту</summary>
    public void SetValue(float value, int id)
    {
        if (id < 7)
        {
            switch (type)
            {
                case 1://bricks

                    break;
                case 2://fakels
                    transform.GetChild(1).GetComponent<Light2D>().pointLightOuterRadius = value;
                    break;
                case 3://death
                    if (id == 0)
                    {
                        GetComponent<Trap>().SetDamage((int)value);
                    }
                    else if (id == 1)
                    {
                        if (GetComponent<DestroyedObj>() != null)
                        {
                            GetComponent<DestroyedObj>().health = (int)value;
                        }
                    }
                    break;
                case 4://gun
                    Gun g = GetComponent<Gun>();
                    if (id == 0)
                    {
                        g._time = value;
                    }
                    else if (id == 1)
                    {
                        if (value == 0)
                            g._toPlayer = false;
                        else
                            g._toPlayer = true;
                    }
                    //else if (id == 2)
                    //{
                    //    g._power = value;
                    //}
                    //else if (id == 3)
                    //{
                    //    if (value == 0)
                    //        g.unselectVelocity = false;
                    //    else
                    //        g.unselectVelocity = true;
                    //}
                    //else if (id == 4)
                    //{
                    //    if (value == 0)
                    //        g.onlyOneAmmo = false;
                    //    else
                    //        g.onlyOneAmmo = true;
                    //}
                    else if (id == 5)
                    {
                        if (typeGun == 1)
                        {
                            //select tnt
                            //if (value == 0)
                            //    g.Ammo = AllUsedComponents.GameObjs[4];
                            //else
                            //    g.Ammo = AllUsedComponents.GameObjs[5];
                        }
                        else
                        {
                            g._stayTime = value;
                        }
                    }
                    break;
                case 5://teleport

                    break;
                case 6://buttono
                    if (id == 1)
                    {
                        GetComponent<ActivatingBaseObject>().speed = value;
                    }
                    else if (id == 4)
                    {
                        GetComponent<ActivatingBaseObject>().ActionType = (ActivationPlateActionType)value;
                    }
                    //select other obj;
                    break;
                case 7://zone
                    if (id == 0)
                    {
                        GetComponent<Zone>().power = value;
                    }
                    break;
                case 8://camZone
                    if (id == 0)
                    {
                        GetComponent<CameraZone>().ZoneCameraScale = value;
                    }
                    break;
                case 9://MovingPlatform
                    MovingPlatform mp = GetComponent<MovingPlatform>();
                    //if (id == 0)
                    //{
                    //    mp.Delta = value;
                    //}
                    //else if (id == 1)
                    //{
                    //    mp.Speed = value;
                    //}
                    break;
                case 10:
                    MovingPlatform mp2 = GetComponent<MovingPlatform>();
                    if (id == 0)
                    {
                        CanvasA.Level.GetComponent<Level>().CameraScale = value;
                    }
                    //else if (id == 1)
                    //{
                    //    mp2.Delta = value;
                    //}
                    //else if (id == 2)
                    //{
                    //    mp2.Speed = value;
                    //}
                    break;
                case 11:
                    if (id == 0)
                    {
                        GetComponent<DestroyedObj>().health = (int)value;
                    }
                    else if (id == 1)
                    {
                        GetComponent<EnemyRing>().SetNeedRange(value);
                    }
                    break;
            }
        }
        else
        {
            if (id == 7)
            {
                GetComponent<RandomActiveObject>().randomValue = value;
            }
        }
    }
    ///<summary>установить сслылку на другой обьект</summary>
    public bool SetObject(GameObject g)
    {
        if (CanvasA.selectedController.id < 7)
        {
            if (type == 5)
            {
                Teleport currTelep = g.GetComponent<Teleport>();
                if (currTelep != null)
                {
                    GetComponent<Teleport>().SecTeleport = currTelep;
                    return true;
                }
            }
            else if (type == 6)
            {
                if (CanvasA.selectedController.id == 0)// using obj
                {
                    GetComponent<ActivatingBaseObject>().FirstUsedObject = g.transform;
                    return true;
                }
                else if (CanvasA.selectedController.id == 2)
                {
                    GetComponent<ActivatingBaseObject>().SecondUsedObject = g.transform;
                    return true;
                }
                else if (CanvasA.selectedController.id == 3)
                {
                    GetComponent<ActivatingBaseObject>().ThirdUsingObject = g.transform;
                    return true;
                }
            }
        }
        else
        {
            if (CanvasA.selectedController.id == 8)
            {
                CanvasA.createLevelSelectedObject.GetComponent<RandomActiveObject>().wall = g;
                CanvasA.RefreshStateRandomActiveObj();
                return true;
            }
        }
        return false;
    }
    ///<summary>скорость изменения трансформа</summary>
    float kp = 0f;
    private void Update()
    {
        CanvasA canv = CanvasA;
        if (isDragging)
        {
            CameraPosition.changeCameraTransform = false;
            Vector2 currPos;
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                currPos = new Vector2(t.position.x, t.position.y);
            }
            else
                currPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.position = firstPosObject + (-startPos + currPos) / 1200f * CameraA.orthographicSize;
        }
        if (this == canv.createLevelSelectedObject)
        {
            if (canv.activeRotateButton1 || canv.activeRotateButton2 || canv.activeScaleXButtDown || canv.activeScaleXButtUp || canv.activeScaleYButtDown || canv.activeScaleYButtUp)
            {
                kp = Mathf.Lerp(kp, 1, Time.unscaledDeltaTime);
            }
            else
                kp = Mathf.Lerp(kp, 0, Time.unscaledDeltaTime * 5f);
            if (canv.activeRotateButton1)
            {
                transform.Rotate(0, 0, Time.unscaledDeltaTime * 60f);
            }
            if (canv.activeRotateButton2)
            {
                transform.Rotate(0, 0, Time.unscaledDeltaTime * -60f);
            }
            if (canv.activeScaleXButtDown)
            {
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 0.01f, Time.unscaledDeltaTime * 1.66f), transform.localScale.y, 1);
            }
            if (canv.activeScaleXButtUp)
            {
                transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 20f, Time.unscaledDeltaTime * 0.15f), transform.localScale.y, 1);
            }
            if (canv.activeScaleYButtDown)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, 0.01f, Time.unscaledDeltaTime * 1.66f), 1);
            }
            if (canv.activeScaleYButtUp)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, 20f, Time.unscaledDeltaTime * 0.15f), 1);
            }
        }
    }

    public void SetParametersToSave()
    {
        OtherParameters = new float[6];
        MainIndex = new int[2];
        OtherIndex = new int[3];
        switch (type)
        {
            case 2://fakels
                OtherParameters[0] = transform.GetChild(1).GetComponent<Light2D>().pointLightOuterRadius;
                break;
            case 3://death
                OtherParameters[0] = GetComponent<Trap>()._damage;
                if (GetComponent<DestroyedObj>() != null)
                {
                    OtherParameters[1] = GetComponent<DestroyedObj>().health;
                }
                break;
            case 4://gun
                Gun g = GetComponent<Gun>();
                OtherParameters[0] = g._time;
                //if (g._toPlayer)
                //    OtherParameters[1] = 1;
                //else
                //    OtherParameters[1] = 0;
                //OtherParameters[2] = g._power;

                //if (g.unselectVelocity)
                //    OtherParameters[3] = 1;
                //else
                //    OtherParameters[3] = 0;

                //if (g.onlyOneAmmo)
                //    OtherParameters[4] = 1;
                //else
                //    OtherParameters[4] = 0;

                if (typeGun == 1)
                {
                    //if (g.Ammo == AllUsedComponents.GameObjs[4])
                    //    OtherParameters[5] = 0;
                    //else
                    //    OtherParameters[5] = 1;
                }
                else
                {
                    OtherParameters[5] = g._stayTime;
                }
                break;
            case 6://buttono
                OtherParameters[0] = GetComponent<ActivatingBaseObject>().speed;
                OtherParameters[1] = (float)GetComponent<ActivatingBaseObject>().ActionType;
                //select other obj;
                break;
            case 7://zone
                OtherParameters[0] = GetComponent<Zone>().power;
                break;
            case 8://camZone
                OtherParameters[0] = GetComponent<CameraZone>().ZoneCameraScale;
                break;
            case 9://MovingPlatform
                MovingPlatform mp = GetComponent<MovingPlatform>();
                //OtherParameters[0] = mp.Delta;
                //OtherParameters[1] = mp.Speed;
                break;
            case 10:
                MovingPlatform mp2 = GetComponent<MovingPlatform>();
                OtherParameters[0] = CanvasA.Level.GetComponent<Level>().CameraScale;
                //OtherParameters[1] = mp2.Delta;
                //OtherParameters[2] = mp2.Speed;
                break;
            case 11:
                OtherParameters[0] = GetComponent<DestroyedObj>().health;
                OtherParameters[1] = GetComponent<EnemyRing>()._needRange;
                break;
        }
        if (GetComponent<RandomActiveObject>() != null)
        {
            mainParams = GetComponent<RandomActiveObject>().randomValue;
            if(GetComponent<RandomActiveObject>().wall != null)
                MainIndex[1] = GetComponent<RandomActiveObject>().wall.GetComponent<MovablePartLevel>().IndexObj;
        }

        if (type == 5)
        {
            if (GetComponent<Teleport>().SecTeleport != null)
            {
                OtherIndex[0] = GetComponent<Teleport>().SecTeleport.GetComponent<MovablePartLevel>().IndexObj;
            }
        }
        else if (type == 6)
        {
            ActivatingBaseObject actPl = GetComponent<ActivatingBaseObject>();
            if (actPl.FirstUsedObject != null)
                OtherIndex[0] = actPl.FirstUsedObject.GetComponent<MovablePartLevel>().IndexObj;
            else
                OtherIndex[0] = 0;
            if (actPl.SecondUsedObject != null)
                OtherIndex[1] =actPl.SecondUsedObject.GetComponent<MovablePartLevel>().IndexObj;
            else
                OtherIndex[1] = 0;
            if (actPl.ThirdUsingObject != null)
                OtherIndex[2] = actPl.ThirdUsingObject.GetComponent<MovablePartLevel>().IndexObj;
            else
                OtherIndex[2] = 0;
        }
        if(transform.parent.name != "Lev")
        {
            MainIndex[0] = transform.parent.GetComponent<MovablePartLevel>().IndexObj;
        }
        else
        {
            MainIndex[0] = 0;
        }

        PosX = transform.position.x;
        PosY = transform.position.y;
        RotZ = transform.localEulerAngles.z;
        ScaleX = transform.localScale.x;
        ScaleY = transform.localScale.y;
    }
    public void SetSavedParams(MovPartLevParams m)
    {
        IndexObj = m.IndexObj;
        OtherParameters = m.OtherParameters;
        mainParams = m.mainParams;
        MainIndex = m.MainIndex;
        OtherIndex = m.OtherIndex;
        type = m.type;
        typeGun = m.typeGun;
        PosX =m.PosX;
        PosY =m.PosY;
        RotZ = m.RotZ;
        ScaleX = m.ScaleX;
        ScaleY = m.ScaleY;
    }
    public void SetParametersAfterOpen()
    {
        switch (type)
        {
            case 2://fakels
                transform.GetChild(1).GetComponent<Light2D>().pointLightOuterRadius=OtherParameters[0] ;
                break;
            case 3://death
                GetComponent<Trap>().SetDamage((int)OtherParameters[0]);
                if (GetComponent<DestroyedObj>() != null)
                {
                    GetComponent<DestroyedObj>().health = (int)OtherParameters[1];
                }
                break;
            case 4://gun
                Gun gun = GetComponent<Gun>();
                gun._time = OtherParameters[0];

                if (OtherParameters[1] == 1)
                    gun._toPlayer = true;
                else
                    gun._toPlayer = false;
                //gun._power = OtherParameters[2];
                //if (OtherParameters[3] == 1)
                //    gun.unselectVelocity = true;
                //else
                //    gun.unselectVelocity = false;

                //if (OtherParameters[4] == 1)
                //    gun.onlyOneAmmo = true;
                //else
                //    gun.onlyOneAmmo = false;

                if (typeGun == 1)
                {
                    //if ( OtherParameters[5] == 0)
                    //    gun.Ammo = AllUsedComponents.GameObjs[4];
                    //else
                    //    gun.Ammo = AllUsedComponents.GameObjs[5];
                }
                else
                {
                    gun._stayTime = OtherParameters[5];
                }
                break;
            case 6://buttono
                GetComponent<ActivatingBaseObject>().speed = OtherParameters[0];
                GetComponent<ActivatingBaseObject>().ActionType = (ActivationPlateActionType)OtherParameters[1];
                //select other obj;
                break;
            case 7://zone
                GetComponent<Zone>().power = OtherParameters[0];
                break;
            case 8://camZone
                GetComponent<CameraZone>().ZoneCameraScale = OtherParameters[0];
                break;
            case 9://MovingPlatform
                MovingPlatform mp = GetComponent<MovingPlatform>();
                //mp.Delta = OtherParameters[0];
                //mp.Speed = OtherParameters[1];
                break;
            case 10:
                MovingPlatform mp2 = GetComponent<MovingPlatform>();
                CanvasA.Level.GetComponent<Level>().CameraScale = OtherParameters[0];
                //mp2.Delta = OtherParameters[1];
                //mp2.Speed = OtherParameters[2];
                break;
            case 11:
                GetComponent<DestroyedObj>().health = (int)OtherParameters[0];
                GetComponent<EnemyRing>().SetNeedRange(OtherParameters[1]);
                break;
        }
        /////////
        RandomActiveObject randAct = null;
        if (mainParams > 0 || MainIndex[1] != 0)
            if (GetComponent<RandomActiveObject>() == null)
                randAct = gameObject.AddComponent<RandomActiveObject>();
            else
                randAct = GetComponent<RandomActiveObject>();
        if (randAct != null)
        {
            randAct.randomValue = mainParams;
            if (MainIndex[1] != 0)
            {
                GameObject g2 = null;
                g2 = FindObjectWithIndex(MainIndex[1]);
                if (g2 != null)
                    randAct.wall = g2;
            }
        }
        ActivatingBaseObject actPl = GetComponent<ActivatingBaseObject>();
        GameObject g = null;
        if (OtherIndex[0] != 0)
            g = FindObjectWithIndex(OtherIndex[0]);
        else g = null;
        if (g != null)
            if (type == 5)
            {
                GetComponent<Teleport>().SecTeleport = g.GetComponent<Teleport>();
            }
            else if (type == 6)
            {
                actPl.FirstUsedObject = g.transform;

            }
        if (OtherIndex[1] != 0)
            g = FindObjectWithIndex(OtherIndex[1]);
        else g = null;
        if (g != null)
            actPl.SecondUsedObject = g.transform;

        if (OtherIndex[2] != 0)
            g = FindObjectWithIndex(OtherIndex[2]);
        else g = null;
        if (g != null)
            actPl.ThirdUsingObject = g.transform;

        if (MainIndex[0] != 0)
            g = FindObjectWithIndex(MainIndex[0]);
        else g = null;
        if (g != null)
           transform.SetParent(g.transform);
        transform.position = new Vector3(PosX, PosY, 0);
        transform.localEulerAngles = new Vector3(0, 0, RotZ);
        transform.localScale = new Vector3(ScaleX, ScaleY, 1);


    }
    GameObject FindObjectWithIndex(int needIndex)
    {
        GameObject g = null;
        Transform lev = CanvasA.Level;
        g = TryFindObjWithId(lev);
        GameObject TryFindObjWithId(Transform t)
        {
            MovablePartLevel m = t.GetComponent<MovablePartLevel>();
            if (m!= null)
            {
                if (m.IndexObj == needIndex)
                {
                    return m.gameObject;
                }
            }
            if (t.childCount > 0)
            {
                for(int i = 0; i < t.childCount; i++)
                {
                    GameObject g2= TryFindObjWithId(t.GetChild(i));
                    if(g2 != null)
                    {
                        return g2;
                    }
                }
            }
            return null;
        }
        return g;
    }

    public void OnSpawn(int id, int list)
    {
        this.ObjId = id;
        this.ListId = list;
        RandomizeIndex();
    }
    public void RandomizeIndex()
    {
        IndexObj = UnityEngine.Random.Range(1, Int32.MaxValue);
    }
}

[Serializable]
public class MovPartLevParams
{
    public int ObjId;
    public int ListId;
    public int IndexObj;
    public float PosX, PosY, RotZ, ScaleX, ScaleY;
    public float[] OtherParameters;
    /// <summary>random value </summary>
    public float mainParams;
    /// <summary>0-parent 1-wall,OtherIndex:/// </summary>
    public int[] MainIndex;
    /// <summary>///type:5 0-OtherTel,type:6 UsingObj,1-RotPoint,3- FinishPos, </summary>
    public int[] OtherIndex;
    ///<summary>тип двиг обьекта 1-brick 2fakel 3death 4gun 5teleport 6button7zone 8 cam zone 9 moving platform 10-center</summary>
    public int type;
    ///<summary>тип пушки 0-normal 1-tnt //or using obj0  have other obj 1 have 2oth obj 2 3-timer</summary>
    public int typeGun;
}
