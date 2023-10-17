using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    public Slider StarsSlider;
    public GameObject StarPrefab;
    public Transform StarParent;
    public bool UseShaking;
    public GameObject imageShaking;

    public void Start()
    {
        int countStart = PlayerPrefs.GetInt("CountStars");
        if (countStart == 0)
            countStart = 20;
        StarsSlider.value = countStart;
        ChangeCountStars();
        if (PlayerPrefs.GetInt("shaking") == 1)
        {
            ChangeShaking();
        }
    }
    public void ChangeCountStars()
    {
        PlayerPrefs.SetInt("CountStars", (int)StarsSlider.value);
        List<GameObject> objToDestroy = new List<GameObject>();
        for (int i = 0; i < StarParent.childCount; i++)
        {
            objToDestroy.Add(StarParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < StarParent.childCount; i++)
        {
            Destroy(objToDestroy[i]);
        }
        for (int i = 0; i < (int)StarsSlider.value; i++)
        {
            Star st = Instantiate(StarPrefab, StarParent).GetComponent<Star>();
            st.maxDeltaX = i*0.5f;
            st.maxDeltaY = i * 0.75f;
        }
    }
    public void ChangeShaking()
    {
        UseShaking = !UseShaking;
        if (UseShaking)
            PlayerPrefs.SetInt("shaking", 1);
        else
            PlayerPrefs.SetInt("shaking", 0);
        imageShaking.SetActive(UseShaking);
    }

}

