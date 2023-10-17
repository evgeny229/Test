using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomActiveObject : MonoBehaviour
{
    public GameObject wall;
    public float randomValue;
    public List<GameObject> ObjectsToDestroy;

    public void RandomObject(float persent)
    {
        float needPersent = persent * randomValue/100;
        float a = Random.Range(0, 100);
        if (a < needPersent)
            if (wall != null)
                Destroy(wall);
        else
        {
            DestroyOtherObj();
            Destroy(gameObject);
        }
    }
    private void DestroyOtherObj()
    {
        if(ObjectsToDestroy!=null)
            for (int i = 0; i < ObjectsToDestroy.Count; i++)
            {
                Destroy(ObjectsToDestroy[i]);
            }
    }
}
