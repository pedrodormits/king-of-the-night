using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Follows multiple players Calculating the center point between them.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    #region VARIABLES
    [Header("POSITION")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime;
    private List<Transform> _players;
    private Vector3 _velocity;
    #endregion
    
    private void Awake() => AddPlayerForCamera();

    private void FixedUpdate() => FollowPlayer();
    
    private void AddPlayerForCamera()
    {
        _players = new List<Transform>();
        
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            _players.Add(player.transform);
    }
    
    private void FollowPlayer()
    {
        if (_players.Count == 0)
            return;

        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
    }
    
    Vector3 GetCenterPoint()
    {
        if (_players.Count == 1)
            return _players[0].position;

        Bounds bounds = new Bounds(_players[0].position, Vector3.zero);
        
        for (int i = 1; i < _players.Count; i++)
        {
            bounds.Encapsulate(_players[i].position);
        }

        return bounds.center;
    }
}