using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float maxDeltaX;
    public float minDeltaX;
    public float maxDeltaY;
    public float minDeltaY;
    Animator anim;
    SpriteRenderer sp;
    CameraPosition CameraPosition;
    public void Construct(CameraPosition cameraPosition)
    {
        CameraPosition = cameraPosition;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        //StartCoroutine(ShowStar());
    }
    IEnumerator ShowStar()
    {
        while (true)
        {
            int randValue = Random.Range(0, 5);
            sp.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
            anim.Rebind();
            if (randValue == 1 || randValue == 2)
                anim.Play("StarAnim2");
            else if (randValue == 3 || randValue == 4)
                anim.Play("starAnim3");
            float posX = Random.Range(minDeltaX, maxDeltaX);
            if (Random.Range(0, 2) == 0)
                posX = -posX;
            float posY = Random.Range(minDeltaY, maxDeltaY);
            if (Random.Range(0, 2) == 0)
                posY = -posY;
            transform.position = CameraPosition.transform.position + new Vector3(posX, posY, 0);
            if (Random.Range(0, 2) == 0)
                transform.eulerAngles = new Vector3(0,0,90f);
            else 
                transform.eulerAngles =Vector3.zero;
            if (randValue == 0)
                yield return new WaitForSeconds(Random.Range(3f, 4f));
            else if (randValue == 1 || randValue == 2)
                yield return new WaitForSeconds(Random.Range(1.2f, 1.5f));
            else if (randValue == 3 || randValue == 4)
                yield return new WaitForSeconds(Random.Range(2f, 3f));
        }
    }
}
