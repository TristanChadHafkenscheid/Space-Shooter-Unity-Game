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
        [SerializeField] private int _damageToPlayer = 15;
        [SerializeField] private GameObject _expPref;

        private PlayerController _player;
        protected SpawnManager _spawnManager;
        private Collider2D _colldier;
        protected AudioManager _audioManager;
        private Rigidbody2D _rigidBody;
        private Vector2 _movement;

        private float _canDamage;
        [SerializeField] float _damageRate;

        protected virtual void Start()
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
        }

        protected virtual void Update()
        {
            CalculateDirection();
        }

        private void FixedUpdate()
        {
            MoveEnemy(_movement);
        }

        protected virtual void MoveEnemy(Vector2 direction)
        {
            _rigidBody.MovePosition((Vector2)transform.position + (direction * _speed * Time.deltaTime));
        }

        protected virtual void CalculateDirection()
        {
            Vector3 direction = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            _rigidBody.rotation = angle;
            direction.Normalize();
            _movement = direction;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
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

        protected virtual void OnCollisionStay2D(Collision2D collision)
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

        protected virtual void EnemyDestroyed()
        {
            _spawnManager.SpawnExp(transform);
            _spawnManager.SpawnExplosion(transform);
            _audioManager.Play("EnemyExplosion");
            gameObject.SetActive(false);
        }
    }
}