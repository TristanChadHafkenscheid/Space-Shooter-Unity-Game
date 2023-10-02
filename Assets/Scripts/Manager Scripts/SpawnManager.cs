﻿using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //[SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private int _enemySpawnTime;
    [SerializeField] private float _offScreenOffset;
    [SerializeField] private ObjectPool _enemyPool;

    private bool _stopSpawning = false;

    private void Start()
    {
        //StartCoroutine(SpawnEnemy());
        InvokeRepeating(nameof(SpawnEnemy), _enemySpawnTime, _enemySpawnTime);
    }

    public void StartSpawning()
    {
        //StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(_enemySpawnTime);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-2.1f, 2.1f), 6.5f, 0);
            int randomPowerup = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    /*private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(_enemySpawnTime);

        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, CalculateSpawnPosition(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }*/

    private void SpawnEnemy()
    {
        GameObject newEnemy = _enemyPool.GetPooledObject();
        if (newEnemy != null)
        {
            newEnemy.SetActive(true);
            Debug.Log("spawn enemy " + newEnemy);
            newEnemy.transform.position = CalculateSpawnPosition();
            newEnemy.transform.rotation = Quaternion.identity;
            newEnemy.SetActive(true);
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        //get positions for top and bottom of screen
        Vector3 topRightOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));
        Vector3 botLeftOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 25));

        Vector3 posToSpawnTop = new Vector3(Random.Range(botLeftOfScreen.x, topRightOfScreen.x), topRightOfScreen.y + _offScreenOffset, 0);
        Vector3 posToSpawnBottom = new Vector3(Random.Range(botLeftOfScreen.x, topRightOfScreen.x), botLeftOfScreen.y - _offScreenOffset, 0);
        Vector3 posToSpawnLeft = new Vector3(botLeftOfScreen.x - _offScreenOffset, Random.Range(botLeftOfScreen.y, topRightOfScreen.y), 0);
        Vector3 posToSpawnRight = new Vector3(topRightOfScreen.x + _offScreenOffset, Random.Range(botLeftOfScreen.y, topRightOfScreen.y), 0);

        Vector3[] directions = { posToSpawnTop, posToSpawnBottom, posToSpawnLeft, posToSpawnRight };

        return directions[Random.Range(0, directions.Length)];
    }
}
