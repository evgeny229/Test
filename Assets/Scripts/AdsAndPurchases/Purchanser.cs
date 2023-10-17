using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;
public class Purchanser : MonoBehaviour
{
    public int buyingId;
    public int[] values;

    private PanelSelectRing PanelSelectRing;
    private CameraInGameScene Camera;
    RingsController AllRings;

    [Inject]
    public void Construct (PanelSelectRing panelSelectRing)
    {
        PanelSelectRing = panelSelectRing;
    }
    public void PurchansedCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "com.floatgames.ballrundungeon.purchansering":
                Donate();
                break;
            case "com.floatgames.ballrundungeon.purchansecreatormode":
                //getCreatorController.GetCreatorMode();
                break;
        }
    }
    private void Donate()
    {
        AllRings.BuyNewRing();
        PanelSelectRing.UpdatePanelSelectRing();
    }

}
