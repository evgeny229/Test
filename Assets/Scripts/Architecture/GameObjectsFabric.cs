using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameObjectsFabric : MonoInstaller
{
    public override void InstallBindings()
    {
    }
    public Traektor[] InstantiateTraectoriaPoints(GameObject prefab, Transform parent, int countPoints)
    {
        Traektor[] resultArray = new Traektor[countPoints];
        for (int i = 0; i < countPoints; i++)
        {
            resultArray[i] = Container.InstantiatePrefab(prefab, parent).GetComponent<Traektor>();
            //resultArray[i].traectoriaId = i;
        }
        return resultArray;
    }
}
