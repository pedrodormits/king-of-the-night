using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Variables
    public static ObjectPool Instance;
    [SerializeField] List<PoolSO> _PoolDatas;
    private Dictionary<GameObject, Queue<GameObject>> _poolDictionary = new();
    #endregion
    
    private void Awake()
    {
        Instance = this;
        CreatePools();
    }
    
    private void CreatePools()
    {
        foreach (PoolSO poolData in _PoolDatas)
        {
            Queue<GameObject> objectPool = new();
            for (int i = 0; i < poolData.PoolSize; i++)
            {
                GameObject obj = Instantiate(poolData.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(poolData.Prefab, objectPool);
        }
    }

    public GameObject GetObject(PoolSO poolData)
    {
        Queue<GameObject> pool = _poolDictionary[poolData.Prefab];
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        return obj;
    }
    
    public void ReturnObject(PoolSO poolData, GameObject obj)
    {
        obj.SetActive(false);

        _poolDictionary[poolData.Prefab].Enqueue(obj);
    }
}