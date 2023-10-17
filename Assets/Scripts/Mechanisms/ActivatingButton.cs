using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingButton : ActivatingBaseObject
{
    [SerializeField] private AudioSource ClickFSX;
    private Gun Gun;
    public override void Start()
    {
        base.Start();
        Gun = FirstUsedObject.GetComponent<Gun>();
    }
    public override void Action()
    {
        base.Action();
        switch (ActionType)
        {
            case ActivationPlateActionType.ReActive:///for button
                ChangeActionButtonReActive();
                break;
            case ActivationPlateActionType.Fire:///for button plate timer
                Fire();
                break;
        }
        Deactive();
    }
    private void Fire()
    {
            GunFire();
    }
    private void GunFire()
    {
        //Gun.CreateAmmo(true);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 9)
        {
            TryPlayMusic();
            Active();
            Destroy(collision.gameObject);
        }
    }
    private void TryPlayMusic()
    {

    }
}
