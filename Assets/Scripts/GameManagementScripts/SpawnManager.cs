using System.Collections;
using UnityEngine;

namespace Managers
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("Pools")]
        [SerializeField] private ObjectPool _enemyPool;
        [SerializeField] private ObjectPool _enemyPurplePool;
        [SerializeField] private ObjectPool _enemyShooterPool;
        [SerializeField] private ObjectPool _expPool;
        [SerializeField] private ObjectPool _playerLaserPool;
        [SerializeField] private ObjectPool _enemyLaserPool;
        [SerializeField] private ObjectPool _explosionPool;

        [Header("Basic Enemy Times")]
        [SerializeField] private float _basicEnemySpawnTime = 2;
        [SerializeField] private float _basicEnemyStartTime = 0;

        [Header("Enemy Purple Times")]
        [SerializeField] private float _enemyPurpleSpawnTime = 5;
        [SerializeField] private float _enemyPurpleStartTime = 15;

        [Header("Shooter Enemy Times")]
        [SerializeField] private float _enemyShooterSpawnTime = 15;
        [SerializeField] private float _enemyShooterStartTime = 30;

        [Space]

        [SerializeField] private float _offScreenOffset;
        [SerializeField] private float _explosionDisableTime;
        [SerializeField] private bool _stopEnemiesSpawning = false;

        private float _basicEnemyTimer;
        private bool _basicEnemyTimeActive = false;

        private float _enemyPurpleTimer;
        private bool _enemyPurpleTimeActive = false;

        private float _enemyShooterTimer;
        private bool _enemyShooterTimeActive = false;

        private bool _stopAllSpawning = false;

        public static SpawnManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        private void Update()
        {
            if (_stopAllSpawning || _stopEnemiesSpawning)
                return;
            BasicEnemyTimer();
            ShooterEnemyTimer();
            EnemyPurpleTimer();
        }

        private void BasicEnemyTimer()
        {
            _basicEnemyTimer += Time.deltaTime;
            if (_basicEnemyTimer >= _basicEnemyStartTime)
                _basicEnemyTimeActive = true;
            if (_basicEnemyTimer >= _basicEnemySpawnTime && _basicEnemyTimeActive)
            {
                SpawnEnemy(_enemyPool);
                _basicEnemyTimer = 0;
            }
        }

        private void EnemyPurpleTimer()
        {
            _enemyPurpleTimer += Time.deltaTime;
            if (_enemyPurpleTimer >= _enemyPurpleStartTime)
                _enemyPurpleTimeActive = true;
            if (_enemyPurpleTimer >= _enemyPurpleSpawnTime && _enemyPurpleTimeActive)
            {
                SpawnEnemy(_enemyPurplePool);
                _enemyPurpleTimer = 0;
            }
        }

        private void ShooterEnemyTimer()
        {
            _enemyShooterTimer += Time.deltaTime;
            if (_enemyShooterTimer >= _enemyShooterStartTime)
                _enemyShooterTimeActive = true;
            if (_enemyShooterTimer >= _enemyShooterSpawnTime && _enemyShooterTimeActive)
            {
                SpawnEnemy(_enemyShooterPool);
                _enemyShooterTimer = 0;
            }
        }

        public void OnPlayerDeath()
        {
            _stopAllSpawning = true;
        }

        private void SpawnEnemy(ObjectPool enemyPool)
        {
            GameObject newEnemy = enemyPool.GetPooledObject();
            if (newEnemy != null)
            {
                newEnemy.transform.SetPositionAndRotation(CalculateSpawnPosition(), Quaternion.identity);
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
                newExp.transform.SetPositionAndRotation(enemyTranform.position, Quaternion.identity);
                newExp.SetActive(true);
            }
        }

        public void SpawnExplosion(Transform enemyTranform)
        {
            StartCoroutine(SpawnExplosionCo(enemyTranform));
        }

        private IEnumerator SpawnExplosionCo(Transform enemyTranform)
        {
            GameObject newExplosion = _explosionPool.GetPooledObject();

            if (newExplosion != null)
            {
                newExplosion.transform.SetPositionAndRotation(enemyTranform.position, Quaternion.identity);
                newExplosion.SetActive(true);
            }

            yield return new WaitForSeconds(_explosionDisableTime);

            newExplosion.SetActive(false);
        }

        public void SpawnPlayerLaser(Transform barrelTrans, Transform playerTrans)
        {
            GameObject laser = _playerLaserPool.GetPooledObject();

            if (laser != null)
            {
                laser.SetActive(true);
                laser.transform.SetPositionAndRotation(barrelTrans.position, playerTrans.rotation);
            }
        }

        public void SpawnEnemyLaser(Transform barrelTrans, Transform playerTrans)
        {
            GameObject laser = _enemyLaserPool.GetPooledObject();

            if (laser != null)
            {
                laser.SetActive(true);
                laser.transform.SetPositionAndRotation(barrelTrans.position, playerTrans.rotation);
            }
        }
    }
}
