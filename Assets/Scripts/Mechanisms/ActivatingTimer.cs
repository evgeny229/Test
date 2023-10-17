using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingTimer : ActivatingBaseObject
{
    private float _imageTimerKp;
    private Gun Gun;
    public override void Start()
    {
        base.Start();
        Gun = FirstUsedObject.GetComponent<Gun>();
        Initialize();
    }
    private void Initialize()
    {
        _imageTimerKp = speed * 0.25f;
    }
    public override void Action()
    {
        base.Action();
        switch (ActionType)
        {
            case ActivationPlateActionType.Fire:///for button plate timer
                Fire();
                break;
        }
    }
    private void Fire()
    {
        AddTimeToAction();
    }
    private void AddTimeToAction()
    {
        _timeToAction += Time.deltaTime;
        CheckNeedFire();
        UpdateImageTime();
    }
    protected void CheckNeedFire()
    {
        if (_timeToAction > speed)
        {
            _timeToAction = 0;
            GunFire();
        }
    }
    private void GunFire()
    {
        //Gun.CreateAmmo(true);
    }
    private void UpdateImageTime()
    {
        activeTimerImages(3, false);
        activeTimerImages(3 - (int)(_timeToAction / _imageTimerKp), true);
        void activeTimerImages(int count, bool active)
        {
            for (int i = 0; i < count; i++)
                transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
