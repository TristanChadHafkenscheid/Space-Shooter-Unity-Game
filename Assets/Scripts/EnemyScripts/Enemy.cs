using Player;
using UnityEngine;
using Managers;
using Audio;
using Attachments;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed = 4;
        [SerializeField] private GameObject _laserPrefab;
        [SerializeField] private int _damageToPlayer = 15;
        [SerializeField] private GameObject _expPref;

        private PlayerController _player;
        private SpawnManager _spawnManager;
        private Collider2D _colldier;
        private AudioManager _audioManager;
        private Rigidbody2D _rigidBody;
        private Vector2 _movement;

        //private float _fireRate = 3f;
        //private float _canFire = -1f;
        //private bool _isShooting = false;

        private float _canDamage;
        [SerializeField] float _damageRate;

        void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            _spawnManager = SpawnManager.instance;
            _audioManager = AudioManager.Instance;

            _colldier = GetComponent<Collider2D>();
            if (_colldier == null)
            {
                Debug.LogError("Colldier2D on enemy is NULL");
            }

            _rigidBody = GetComponent<Rigidbody2D>();

            if (_rigidBody == null)
            {
                Debug.LogError("Rigidbody on player is NULL");
            }

            //_isShooting = true;
        }

        void Update()
        {
            CalculateDirection();

            //not using shooting for now
            //CalculateShooting();
        }

        private void FixedUpdate()
        {
            MoveEnemy(_movement);
        }

        private void MoveEnemy(Vector2 direction)
        {
            _rigidBody.MovePosition((Vector2)transform.position + (direction * _speed * Time.deltaTime));
        }

        private void CalculateDirection()
        {
            Vector3 direction = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _rigidBody.rotation = angle;
            direction.Normalize();
            _movement = direction;
        }

        //private void CalculateShooting()
        //{
        //    if (Time.time > _canFire && _isShooting == true)
        //    {
        //        _fireRate = Random.Range(3f, 7f);
        //        _canFire = Time.time + _fireRate;
        //        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        //        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        //        for (int i = 0; i < lasers.Length; i++)
        //        {
        //            lasers[i].AssignEnemyLaser();
        //        }
        //    }
        //}

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("BigLaser"))
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                EnemyDestroyed();
            }
            else if (collision.gameObject.CompareTag("PlayerLaser"))
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }

                collision.gameObject.SetActive(false);

                EnemyDestroyed();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_player != null && Time.time >= _canDamage)
                {
                    _canDamage = Time.time + _damageRate;
                    _player.Damage(_damageToPlayer);
                }
            }
            if (collision.gameObject.CompareTag("Attachment"))
            {
                if (_player != null && Time.time >= _canDamage)
                {
                    _canDamage = Time.time + _damageRate;
                    collision.gameObject.GetComponent<ShipAttachment>().Damage(_damageToPlayer);
                }
            }
        }

        private void EnemyDestroyed()
        {
            _spawnManager.SpawnExp(transform);
            _spawnManager.SpawnExplosion(transform);
            _audioManager.Play("EnemyExplosion");
            gameObject.SetActive(false);
        }
    }
}