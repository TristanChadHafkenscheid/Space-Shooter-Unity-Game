using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTileManager : MonoBehaviour
{
    [SerializeField] private Transform[] _backgroundArray;
    [SerializeField] float _tileDistance = 20.48f;
    [SerializeField] float _safeDistanceX  = 6;
    [SerializeField] float _safeDistanceY = 2.5f;

    private int _farthestTile = 0;
    private int _currentTile = 0;
    private Transform _playerTransform;

    private bool _isFarTileSet = false;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _currentTile = FindCurrentTile();
    }

    void Update()
    {
        Debug.Log("Current tile - player: " + (_backgroundArray[_currentTile].position - _playerTransform.position));

        //_currentTile = FindCurrentTile();

        //moving left
        if (_backgroundArray[_currentTile].position.x - _playerTransform.position.x >=
            _safeDistanceX && _isFarTileSet == false)
        {
            _currentTile = FindCurrentTile();
            _farthestTile = FindFarthestTile();

            SetFarTile(Vector3.left);
        }
        else if (_backgroundArray[_currentTile].position.x - _playerTransform.position.x <
            _safeDistanceX)
        {
            _isFarTileSet = false;
        }

        //moving right
        if (_backgroundArray[_currentTile].position.x - _playerTransform.position.x <=
            -_safeDistanceX && _isFarTileSet == false)
        {
            _currentTile = FindCurrentTile();
            _farthestTile = FindFarthestTile();

            SetFarTile(Vector3.right);
        }
        else if (_backgroundArray[_currentTile].position.x - _playerTransform.position.x >
            -_safeDistanceX)
        {
            _isFarTileSet = false;
        }

        //moving up
        if (_backgroundArray[_currentTile].position.y - _playerTransform.position.y <=
            -_safeDistanceY && _isFarTileSet == false)
        {
            _currentTile = FindCurrentTile();
            _farthestTile = FindFarthestTile();

            SetFarTile(Vector3.up);
        }
        else if (_backgroundArray[_currentTile].position.y - _playerTransform.position.y >
            -_safeDistanceY)
        {
            _isFarTileSet = false;
        }

        //moving down
        if (_backgroundArray[_currentTile].position.y - _playerTransform.position.y >=
            _safeDistanceY && _isFarTileSet == false)
        {
            _currentTile = FindCurrentTile();
            _farthestTile = FindFarthestTile();

            SetFarTile(Vector3.down);
        }
        else if (_backgroundArray[_currentTile].position.y - _playerTransform.position.y <
            _safeDistanceY)
        {
            _isFarTileSet = false;
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

    private void SetFarTile(Vector3 tileDirection)
    {
        _backgroundArray[_farthestTile].position = _backgroundArray[_currentTile].position
            + tileDirection * _tileDistance;
        _isFarTileSet = true;
    }
}