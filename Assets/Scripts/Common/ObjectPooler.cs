using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }
    public int SpawnedObjectCount = 0;

    public List<Pool> ObjectPools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in ObjectPools)
        {
            var queue = new Queue<GameObject>();
            for (var i = 0; i < pool.Size; i++)
            {
                var obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            PoolDictionary.Add(pool.GameObjectTag, queue);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            return null;
        }

        SpawnedObjectCount++;
        var obj = PoolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        PoolDictionary[tag].Enqueue(obj);

        return obj;
    }
}
