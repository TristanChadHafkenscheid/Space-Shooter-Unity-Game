using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTileManager : MonoBehaviour
{
    [SerializeField] private Transform[] _backgroundArray;
    [SerializeField] float _tileDistance = 20.48f;
    private int _farthestTile = 0;
    private int _currentTile = 0;
    private Transform _playerTransform;
    [SerializeField] float _safeDistance;
    private bool _isFarRightTileSet = false;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        FindCurrentTile();
    }

    void Update()
    {
        Debug.Log("Current tile pos - player pos: " + (_backgroundArray[_currentTile].position.x - _playerTransform.position.x));
        Debug.Log("Current tile pos - safe distance: " + (_safeDistance));

        _currentTile = FindCurrentTile();

        if (_backgroundArray[_currentTile].position.x - _playerTransform.position.x >=
            _safeDistance && _isFarRightTileSet == false)
        {
            _currentTile = FindCurrentTile();
            _farthestTile = FindFarthestTile();

            SetFarRightTile();
        }
        else if (_backgroundArray[_currentTile].position.x - _playerTransform.position.x <
            _safeDistance)
        {
            _isFarRightTileSet = false;
        }
    }

    private int FindFarthestTile()
    {
        float farthestDistance = 0;

        for (int i = 0; i < _backgroundArray.Length; i++)
        {
            float dist = Vector3.Distance(_backgroundArray[_currentTile].position, _backgroundArray[i].position);
            if (dist >= farthestDistance)
            {
                _farthestTile = i;
                farthestDistance = dist;
            }
        }
        Debug.Log("Farthest tile is " + _farthestTile);
        return _farthestTile;
    }

    private int FindCurrentTile()
    {
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < _backgroundArray.Length; i++)
        {
            float dist = Vector3.Distance(_backgroundArray[i].position, _playerTransform.position);
            if (dist <= closestDistance)
            {
                _currentTile = i;
                closestDistance = dist;
            }
        }
        Debug.Log("Current tile is " + _currentTile);
        return _currentTile;
    }

    private void SetFarRightTile()
    {
        _backgroundArray[_farthestTile].position = _backgroundArray[_currentTile].position
            + Vector3.left * _tileDistance;
        _isFarRightTileSet = true;
    }
}