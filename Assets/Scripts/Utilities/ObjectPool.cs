using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        if (poolDictionary[prefab].Count == 0)
        {
            GameObject newObj = Instantiate(prefab, position, rotation);
            newObj.SetActive(true);
            return newObj;
        }

        GameObject obj = poolDictionary[prefab].Dequeue();
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public void ReturnObject(GameObject prefab, GameObject obj, float delay = 0f)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        if (delay > 0f)
        {
            // For delayed return, we could use coroutine here
            // For simplicity, immediate return
        }
        poolDictionary[prefab].Enqueue(obj);
    }
}