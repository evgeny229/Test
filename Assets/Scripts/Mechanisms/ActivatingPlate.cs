using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingPlate : ActivatingBaseObject
{
    private Gun Gun;
    private int _shapeCount;
    public override void Start()
    {
        base.Start();
        Gun = FirstUsedObject.GetComponent<Gun>();
    }
    private void Update()
    {
        if (ActionType == ActivationPlateActionType.ReActive)
            if (ActivatedCollider != null)
            {
                ActivatedCollider.enabled = _active;
                VisualActiveButtonWithCollider();
            }
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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 13)
        {
            PressOnPlate(0.5f);
            Active();
            IncreaseShapeCount();
            Destroy(collision.gameObject);
        }
    }
    private void PressOnPlate(float scaleY)
    {
        ButtonScalablePart.localScale = new Vector3(1, scaleY, 1);
    }
    private void IncreaseShapeCount()
    {
        _shapeCount++;
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (_shapeCount > 0 && (collision.gameObject.layer == 6 || collision.gameObject.layer == 13))
            DegreaseShapeCount();
    }
    private void DegreaseShapeCount()
    {
        _shapeCount--;
        CheckShapeCount();
    }
    private void CheckShapeCount()
    {
        if (_shapeCount == 0)
        {
            Deactive();
            PressOnPlate(1f);
        }
    }
}
