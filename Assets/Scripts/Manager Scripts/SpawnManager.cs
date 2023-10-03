﻿using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private int _enemySpawnTime;
    [SerializeField] private float _offScreenOffset;
    [SerializeField] private ObjectPool _enemyPool;
    [SerializeField] private ObjectPool _expPool;
    [SerializeField] private ObjectPool _playerLaserPool;
    [SerializeField] private ObjectPool _explosionPool;

    private bool _stopSpawning = false;

    public static SpawnManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), _enemySpawnTime, _enemySpawnTime);
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

    private void SpawnEnemy()
    {
        GameObject newEnemy = _enemyPool.GetPooledObject();
        if (newEnemy != null)
        {
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

    public void SpawnExp(Transform enemyTranform)
    {
        GameObject newExp = _expPool.GetPooledObject();
        if (newExp != null)
        {
            newExp.transform.position = enemyTranform.position;
            newExp.transform.rotation = Quaternion.identity;
            newExp.SetActive(true);
        }
    }

    public void SpawnExplosion(Transform enemyTranform)
    {
        GameObject newExplosion = _expPool.GetPooledObject();
        if (newExplosion != null)
        {
            newExplosion.transform.position = enemyTranform.position;
            newExplosion.transform.rotation = Quaternion.identity;
            newExplosion.SetActive(true);
            //Invoke(nameof)
        }
    }

    public void SpawnPlayerLaser(Transform barrelTrans, Transform playerTrans)
    {
        GameObject laser = _playerLaserPool.GetPooledObject();
        if (laser != null)
        {
            laser.SetActive(true);
            laser.transform.position = barrelTrans.position;
            laser.transform.rotation = playerTrans.rotation;
        }
    }

    private void DisableExplosion(GameObject explosion)
    {
        explosion.SetActive(false);
    }
}
