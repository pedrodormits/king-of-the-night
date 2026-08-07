using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }
    
    public static ObjectPooler Instance;
    public List<Pool> Pools;
    private Dictionary<string, Queue<GameObject>> PoolDictionary;
    private Dictionary<GameObject,string> _objectTags;

    private void Awake()
    {
        Instance = this;
        _objectTags = new Dictionary<GameObject,string>();
    }

    private void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                _objectTags.Add(obj, pool.Tag);
                objectPool.Enqueue(obj);
            }
            PoolDictionary.Add(pool.Tag, objectPool);
        }
    }
    
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool doesn't exist");
            return null;
        }
        
        if(PoolDictionary[tag].Count == 0)
        {
            Debug.Log("Pool empty");
            return null;
        }
        
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.SetActive(true);
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        return objectToSpawn;
    }
    
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        string tag = _objectTags[obj];
        PoolDictionary[tag].Enqueue(obj);
    }
}