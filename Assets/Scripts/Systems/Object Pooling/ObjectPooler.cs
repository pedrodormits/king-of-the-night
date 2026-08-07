using System.Collections.Generic;
using UnityEngine;

/// <summary> ObjectPooler manages reusable GameObjects to improve performance.
/// Instead of constantly creating and destroying objects, this system creates
/// </summary> a fixed amount of objects at the start and reuses them when needed.
public class ObjectPooler : MonoBehaviour
{
    // Serializable class used to define a pool in the Unity Inspector.
    // Each pool has a tag, a prefab, and the amount of objects it should contain.
    [System.Serializable]
    public class Pool
    {
        public string Tag; // Unique name used to identify this pool
        public GameObject Prefab; // Object prefab that will be created
        public int Size; // Amount of objects created for this pool
    }
    
    public static ObjectPooler Instance; // Singleton instance so other scripts can easily access the ObjectPooler.
    public List<Pool> Pools; // List of all pools configured in the Unity Inspector.
    
    // Dictionary that stores pools using their tag as the key.
    // A Queue is used because objects are taken and returned in order.
    private Dictionary<string, Queue<GameObject>> PoolDictionary;
    
    // Dictionary that keeps track of which pool tag belongs to each object.
    // This is needed when returning an object to the correct pool.
    private Dictionary<GameObject,string> _objectTags;

    private void Awake()
    {
        Instance = this; // Set this ObjectPooler as the global instance.
        _objectTags = new Dictionary<GameObject,string>(); // Create the dictionary that stores object tags.
    }

    private void Start()
    {
        // Create the dictionary that contains all object pools.
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in Pools) // Loop through every pool defined in the Inspector.
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); // Create a new queue for this specific pool.
            for (int i = 0; i < pool.Size; i++) // Pre-create the required amount of objects.
            {
                GameObject obj = Instantiate(pool.Prefab); // Create a new object from the prefab.
                obj.SetActive(false); // Disable the object until it is needed.
                _objectTags.Add(obj, pool.Tag); // Store the object's pool tag.
                objectPool.Enqueue(obj); // Add the object to the pool queue.
            }
            PoolDictionary.Add(pool.Tag, objectPool); // Add the completed pool to the main dictionary.
        }
    }
    
    /// <summary>
    /// Gets an inactive object from a pool and activates it.
    /// The object is positioned and rotated before being returned.
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag)) // Check if a pool with this tag exists.
        {
            Debug.Log("Pool doesn't exist");
            return null;
        }
        
        if(PoolDictionary[tag].Count == 0) // Check if there are available objects in the pool.
        {
            Debug.Log("Pool empty");
            return null;
        }
        
        GameObject objectToSpawn = PoolDictionary[tag].Dequeue(); // Remove the first available object from the queue.
        objectToSpawn.transform.SetPositionAndRotation(position, rotation); // Set the object's position and rotation.
        objectToSpawn.SetActive(true); // Enable the object in the scene.
        
        // Check if the object has a component that implements IPooledObject.
        // If it does, call its spawn event.
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        return objectToSpawn;
    }
    
    /// <summary>
    /// Returns an object back into its original pool.
    /// The object is disabled and stored for future reuse.
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // Disable the object instead of destroying it.
        string tag = _objectTags[obj]; // Find the pool tag belonging to this object.
        PoolDictionary[tag].Enqueue(obj); // Add the object back to its pool queue.
    }
}