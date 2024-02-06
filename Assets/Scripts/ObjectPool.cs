using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public List<GameObject> laserPooledObjects;
    public List<GameObject> tripleLaserPoolObject;
    public GameObject laserObjectToPool, tripleLaserObjectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        laserPooledObjects = new List<GameObject>();
        tripleLaserPoolObject = new List<GameObject>();
        GameObject tmp_laser;
        GameObject tmp_tripleLaser;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp_laser = Instantiate(laserObjectToPool);
            tmp_tripleLaser = Instantiate(tripleLaserObjectToPool);
            tmp_laser.SetActive(false);
            tmp_tripleLaser.SetActive(false);
            laserPooledObjects.Add(tmp_laser);
            tripleLaserPoolObject.Add(tmp_tripleLaser);
        }
    }

    public GameObject GetLaserPooledObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(laserPooledObjects[i] != null && !laserPooledObjects[i].activeInHierarchy)
            {
                return laserPooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetTripleLaserPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (tripleLaserPoolObject[i] != null && !tripleLaserPoolObject[i].activeInHierarchy)
            {
                return tripleLaserPoolObject[i];
            }
        }
        return null;
    }
}
