using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ObjectPool
{
    private GameObject Parent;
    private PoolableObject Prefab;
    private int Size;
    public List<PoolableObject> AvailableObjects;

    private ObjectPool(PoolableObject Prefab, int Size)
    {
        this.Prefab = Prefab;
        this.Size = Size;
        AvailableObjects = new List<PoolableObject>(Size);
    }

    public static ObjectPool CreateInstance(PoolableObject Prefab, int Size)
    {
        ObjectPool pool = new ObjectPool(Prefab, Size);

        //GameObject poolObject = new GameObject(Prefab.name + "Pool");
        //pool.CreateObjects(poolObject.transform, Size);
        pool.Parent = new GameObject(Prefab + " Pool");
        pool.CreateObjects();

        return pool;
    }
    /*
    private void CreateObjects(Transform parent, int Size)
    {
        for(int i = 0; i < Size; i++)
        {
            PoolableObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;  
            poolableObject.gameObject.SetActive(false);
        }
    }
    */

    private void CreateObjects()
    {
        for (int i = 0; i < Size; i++)
        {
            CreateObject();
        }
    }

    private void CreateObject()
    {
        PoolableObject poolableObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity, Parent.transform);
        poolableObject.Parent = this;
        poolableObject.gameObject.SetActive(false); // PoolableObject handles re-adding the object to the AvailableObjects
    }

    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        AvailableObjects.Add(poolableObject);
    }
    /*
    public PoolableObject GetObject()
    {
        if(AvailableObjects.Count > 0)
        {
            PoolableObject instance = AvailableObjects[0];
            AvailableObjects.RemoveAt(0);

            instance.gameObject.SetActive(true);

            return instance;
        } else
        {
            Debug.LogError($"Nie mozna otrzymac obiektu z puli \"{Prefab.name}\" Pool.");
            return null;
        }
    }
    */
    public PoolableObject GetObject()
    {
        if (AvailableObjects.Count == 0) // auto expand pool size if out of objects
        {
            CreateObject();
            Debug.Log("Expanded pool");
        }

        PoolableObject instance = AvailableObjects[0];

        AvailableObjects.RemoveAt(0);

        instance.gameObject.SetActive(true);

        return instance;
    }
}
