using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTileManager : MonoBehaviour
{
    [SerializeField] private Transform[] _backgroundArray;
    [SerializeField] float _tileDistance = 20.48f;
    private int _farthestTile = 0;
    private int _closestTile = 0;
    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;

        FindClosestTile();
    }

    void Update()
    {
        Debug.Log("current pos " + (_backgroundArray[_closestTile].position.x - _playerTransform.position.x));
        if (_backgroundArray[_closestTile].position.x - _playerTransform.position.x < 5)
        {
            FindClosestTile();
            _farthestTile = FindFarthestTile();
            _backgroundArray[_farthestTile].position = _backgroundArray[_closestTile].position
                - Vector3.left * _tileDistance;
        }
    }

    private int FindFarthestTile()
    {
        float farthestDistance = 0;

        for (int i = 0; i < _backgroundArray.Length; i++)
        {
            float dist = Vector3.Distance(_backgroundArray[i].position, _playerTransform.position);
            if (dist >= farthestDistance)
            {
                _farthestTile = i;
                farthestDistance = dist;
            }
        }
        Debug.Log("Farthest tile is " + _farthestTile);
        return _farthestTile;
    }

    private int FindClosestTile()
    {
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < _backgroundArray.Length; i++)
        {
            float dist = Vector3.Distance(_backgroundArray[i].position, _playerTransform.position);
            if (dist <= closestDistance)
            {
                _closestTile = i;
                closestDistance = dist;
            }
        }
        Debug.Log("Closest tile is " + _closestTile);
        return _closestTile;
    }
}
