using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages multiple object pools to avoid frequent instantiation and destruction of GameObjects.
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        // Unique identifier used to access this pool.
        public string Tag;
        
        // Prefab that will be instantiated and stored in the pool.
        public GameObject Prefab;
        
        // Number of objects that will be created when the pool is initialized.
        public int Size;
    }
    
    // Singleton instance for easy global access.
    public static ObjectPooler Instance;
    
    // List of all pools configured in the Inspector.
    public List<Pool> Pools;
    
    // Stores all pooled objects, grouped by their tag.
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    
    // Maps each pooled object back to its pool tag.
    // This allows ReturnObject() to know which queue the object belongs to.
    private Dictionary<GameObject,string> _objectTags;

    private void Awake()
    {
        Instance = this;
        _objectTags = new Dictionary<GameObject,string>();
    }

    private void Start()
    {
        // Create the dictionary that will store every pool.
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        // Initialize each pool defined in the Inspector.
        foreach (Pool pool in Pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            // Instantiate the configured amount of objects.
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                
                // Disable the object so it is ready for future use.
                obj.SetActive(false);
                
                // Store which pool this object belongs to.
                _objectTags.Add(obj, pool.Tag);
                
                // Add the object to the pool.
                objectPool.Enqueue(obj);
            }
            
            // Register the completed pool using its tag.
            PoolDictionary.Add(pool.Tag, objectPool);
        }
    }

    /// <summary>
    /// Retrieves an inactive object from the requested pool,
    /// positions it, activates it, and returns it.
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // Check if the requested pool exists.
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool doesn't exist");
            return null;
        }
        
        // Prevent trying to dequeue from an empty pool.
        if(PoolDictionary[tag].Count == 0)
        {
            Debug.Log("Pool empty");
            return null;
        }
        
        // Retrieve the next available object.
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue();
        
        // Move the object to its spawn position.
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        
        // Enable the object so it becomes active in the scene.
        objectToSpawn.SetActive(true);
        
        // Notify the object that it has been spawned.
        // This is typically used to reset timers, health, velocity, etc.
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        return objectToSpawn;
    }
    
    /// <summary>
    /// Returns an object back to its original pool.
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        string tag = _objectTags[obj];
        PoolDictionary[tag].Enqueue(obj);
    }
}