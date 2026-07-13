using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    #region VARIABLES
    [Header("POSITION")]
    [SerializeField] private Vector3 _Offset;
    [SerializeField] private float _SmoothTime;
    [SerializeField] private List<Transform> _Players;
    private Vector3 _velocity;
    #endregion
    
    private void FixedUpdate() => FollowPlayer();
    
    private void FollowPlayer()
    {
        if (_Players.Count == 0)
        {
            return;
        }

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _Offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _SmoothTime);
    }
    
    Vector3 GetCenterPoint()
    {
        if (_Players.Count == 1)
        {
            return _Players[0].position;
        }

        Bounds bounds = new Bounds(_Players[0].position, Vector3.zero);
        for (int i = 1; i < _Players.Count; i++)
        {
            bounds.Encapsulate(_Players[i].position);
        }

        return bounds.center;
    }
}