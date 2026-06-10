using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    private static readonly Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();

    public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.TryGetValue(prefab, out Queue<GameObject> prefabPool) || prefabPool.Count == 0)
        {
            return Object.Instantiate(prefab, position, rotation);
        }

        GameObject obj = prefabPool.Dequeue();
        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public static void Return(GameObject prefab, GameObject obj)
    {
        if (!pool.TryGetValue(prefab, out Queue<GameObject> prefabPool))
        {
            Object.Destroy(obj);
            return;
        }

        obj.SetActive(false);
        prefabPool.Enqueue(obj);
    }
}