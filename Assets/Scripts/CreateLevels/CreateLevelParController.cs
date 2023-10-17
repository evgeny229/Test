using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
public class CreateLevelParController : MonoBehaviour
{
    ///<summary>текст параметра</summary>
    public Text textNameParameter;
    ///<summary>текст значения параметра</summary>
    public Text textValueParameter;
    ///<summary>само значение</summary>
    public float value;
    ///<summary>максимальное значение</summary>
    public float maxValue;
    ///<summary>тип</summary>
    public int type;//0-int 1-float 2-bool 3-select obj
    ///<summary>коэф скорости изменен</summary>
    float kpChange = 0f;
    ///<summary>ид контроллера</summary>
    public int id;
    CanvasA CanvasA;
    [Inject]
    public void Construct(CanvasA canvasA)
    {
        CanvasA = canvasA;
    }
    ///<summary>только изменение значения</summary>
    private void Update()
    {
        kpChange = Mathf.Lerp(kpChange, 0, Time.unscaledDeltaTime * 0.1f);
        if (LeftPressed || RightPressed)
            kpChange = Mathf.Lerp(kpChange, 1, Time.unscaledDeltaTime * 0.5f);
        if (LeftPressed)
        {
            value = Mathf.Lerp(value, -0.1f, Time.unscaledDeltaTime * kpChange);
            if (value < 0)
                value = 0;
            if (type == 2)
                value = 0;
            SetValue();
        }
        if (RightPressed)
        {
            value = Mathf.Lerp(value, maxValue + 0.1f, 0.2f* Time.unscaledDeltaTime * kpChange);
            if (type == 2)
                value = 1;
            if (value > maxValue)
                value = maxValue;
            SetValue();
        }
    }
    ///<summary>задать все нужные параметры</summary>
    public void SetParameters(string name, float defaultValue,float maxValue, int type)
    {
        this.type = type;
        this.maxValue = maxValue;
        value = defaultValue;
        textNameParameter.text = name;
        LeftPressed = false;
        RightPressed = false;
        SetValue();
    }
    ///<summary>установить значение на обьекте и обновить текст</summary>
    public void SetValue()
    {
        switch (type)
        {
            case 2:
                if (value == 0)
                    textValueParameter.text = "false";
                if (value == 1)
                    textValueParameter.text = "true";
                break;
            case 1:
                float temp = ((float)((int)(value * 100)))/100;
                textValueParameter.text ="" + temp;
                break;
            case 0:
                textValueParameter.text = "" + (int)value;
                break;
            case 3:
                textValueParameter.text = "null";
                break;
        }
        CanvasA.createLevelSelectedObject.SetValue(value, id);
    }

    public bool LeftPressed;
    public bool RightPressed;
    public void LeftDown()
    {
        if (type != 3)
        {
            LeftPressed = true;
        }
    }
    public void LeftUp()
    {
        if (type != 3)
        {
            LeftPressed = false;
        }
    }
    public void RightDown()
    {
        if (type != 3)
        {
            RightPressed = true;
        }
    }
    public void RightUp()
    {
        if (type != 3)
        {
            RightPressed = false;
        }
    }

    public void LeftClick()
    {
        //Delete
        if (type == 3)
        {
            CanvasA.selectedController = null;
            CanvasA.createLevelSelectedObject.SelectNullRef(id);
            textValueParameter.text = "null";
            CanvasA.isSelectModeEnabled = false;
        }
    }
    public void RightClick()
    {
        //select
        if (type == 3)
        {
            CanvasA.isSelectModeEnabled = !CanvasA.isSelectModeEnabled;
            if (CanvasA.isSelectModeEnabled)
            {
                textValueParameter.text = "???";
                CanvasA.selectedController = this;
            }
        }
    }
}
