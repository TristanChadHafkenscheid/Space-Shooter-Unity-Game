using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundTileManager : MonoBehaviour
{
    [SerializeField] private Transform[] backgroundArray;
    [SerializeField] private float tileDistance = 20.48f;

    private Transform _playerTransform;
    private GameManager _gameManager;

    private float _lastPositionX;
    private float _lastPositionY;

    private void Start()
    {
        _playerTransform = Player.instance.GetComponent<Transform>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Update()
    {
        //if gameover is true then return
        //if (_gameManager.game)
        CheckBoundary();
    }

    private IEnumerable<Transform> GetRightColumn()
    {
        var column = new Transform[3];

        var minimumX = backgroundArray.Select(background => background.position.x).Prepend(Mathf.Infinity).Min();

        var c = 0;
        foreach (var background in backgroundArray)
        {
            if (Math.Abs(background.position.x - minimumX) < 0.2f)
            {
                column[c++] = background;
            }
        }

        return column;
    }
    
    private IEnumerable<Transform> GetLeftColumn()
    {
        var column = new Transform[3];

        var maximumX = backgroundArray.Select(background => background.position.x).Prepend(-Mathf.Infinity).Max();

        var c = 0;
        foreach (var background in backgroundArray)
        {
            if (Math.Abs(background.position.x - maximumX) < 0.2f)
            {
                column[c++] = background;
            }
        }

        return column;
    }
    
    private IEnumerable<Transform> GetTopLine()
    {
        var line = new Transform[3];

        var maximumY = backgroundArray.Select(background => background.position.y).Prepend(-Mathf.Infinity).Max();

        var c = 0;
        foreach (var background in backgroundArray)
        {
            if (Math.Abs(background.position.y - maximumY) < 0.2f)
            {
                line[c++] = background;
            }
        }

        return line;
    }
    
    private IEnumerable<Transform> GetBottomLine()
    {
        var line = new Transform[3];

        var minimumY = backgroundArray.Select(background => background.position.y).Prepend(Mathf.Infinity).Min();

        var c = 0;
        foreach (var background in backgroundArray)
        {
            if (Math.Abs(background.position.y - minimumY) < 0.2f)
            {
                line[c++] = background;
            }
        }

        return line;
    }

    private void CheckBoundary()
    {
        // Move Left
        if (_lastPositionX - _playerTransform.position.x < -tileDistance)
        {
            foreach (var background in GetRightColumn())
            {
                background.position += Vector3.right * (tileDistance * 3);
            }
            
            _lastPositionX = _playerTransform.position.x;
        }
        
        // Move Right
        if (_lastPositionX - _playerTransform.position.x > tileDistance)
        {
            foreach (var background in GetLeftColumn())
            {
                background.position += Vector3.left * (tileDistance * 3);
            }
            
            _lastPositionX = _playerTransform.position.x;
        }
        
        // Move Up
        if (_lastPositionY - _playerTransform.position.y > tileDistance)
        {
            foreach (var background in GetTopLine())
            {
                background.position += Vector3.down * (tileDistance * 3);
            }
            
            _lastPositionY = _playerTransform.position.y;
        }
        
        // Move Down
        if (!(_lastPositionY - _playerTransform.position.y < -tileDistance)) return;
        {
            foreach (var background in GetBottomLine())
            {
                background.position += Vector3.up * (tileDistance * 3);
            }
            
            _lastPositionY = _playerTransform.position.y;
        }
    }
}
