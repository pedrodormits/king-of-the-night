using UnityEngine;

[CreateAssetMenu(menuName = "Pooling/Pool")]
public class PoolSO : ScriptableObject
{
    public GameObject Prefab;
    public int PoolSize;
}