using UnityEngine;
using UnityEngine.Events;

public class test2 : MonoBehaviour
{
    [SerializeField]private UnityEvent _event;

    void Start()
    {
        _event.Invoke();
    }
}
