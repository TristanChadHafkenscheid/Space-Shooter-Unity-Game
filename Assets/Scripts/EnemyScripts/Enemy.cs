using Player;
using UnityEngine;
using Managers;
using Audio;
using Attachments;
using DG.Tweening;
using MoreMountains.Tools;
using System.Collections;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _speed = 4;
        [SerializeField] private int _damageToPlayer = 15;
        [SerializeField] private GameObject _expPref;
        [SerializeField] private int _health = 1;
        [SerializeField] private Color _damageColour;
        [SerializeField] float _damageRate;

        private int _initHealth;
        private PlayerController _player;
        protected SpawnManager _spawnManager;
        private CompanionManager _companionManager;
        private Collider2D _colldier;
        protected AudioManager _audioManager;
        private Rigidbody2D _rigidBody;
        private Vector2 _movement;
        private SpriteRenderer _sprite;
        private float _canDamage;

        private Coroutine _damageOverTimeCo;

        private void Awake()
        {
            _initHealth = _health;
        }
        private void OnEnable()
        {
            _health = _initHealth;
        }

        protected virtual void Start()
        {
            _player = PlayerController.instance;
            _spawnManager = SpawnManager.instance;
            _audioManager = AudioManager.Instance;

            _companionManager = _player.GetComponent<CompanionManager>();
            if (_companionManager == null)
            {
                Debug.LogError("Companion Manager is NULL");
            }

            _colldier = GetComponent<Collider2D>();
            if (_colldier == null)
            {
                Debug.LogError("Colldier2D on enemy is NULL");
            }

            _rigidBody = GetComponent<Rigidbody2D>();
            if (_rigidBody == null)
            {
                Debug.LogError("Rigidbody on enemy is NULL");
            }

            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite == null)
            {
                Debug.LogError("Sprite on enemy is NULL");
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
            if (collision.gameObject.CompareTag("PlayerLaser"))
            {
                _health -= collision.gameObject.GetComponent<Laser>().DamageToEnemy;
                DamageVisuals();

                if (_health <= 0)
                {
                    _player.AddScore(10);
                    collision.gameObject.SetActive(false);
                    EnemyDestroyed();
                }
            }
            else if (collision.gameObject.CompareTag("BigLaser"))
            {
                _health -= _player.BigLaserDamage;
                DamageVisuals();

                if (_health <= 0)
                {
                    _player.AddScore(10);
                    EnemyDestroyed();
                }
            }

            else if (collision.gameObject.CompareTag("WaterJet"))
            {
                _health -= _companionManager.WaterJetDamage;
                DamageVisuals();

                if (_health <= 0)
                {
                    _player.AddScore(10);
                    EnemyDestroyed();
                }
            }

            else if (collision.gameObject.CompareTag("AstronautVortex"))
            {
                StartCoroutine(ActivateDamageOverTime(_companionManager.AstronautVortexDamage, _companionManager.AstronautVortexDamageInterval, _companionManager.AstronautVortexActiveTimer));
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

        private void DamageVisuals()
        {
            _sprite.DOKill();
            _sprite.color = Color.white;
            _sprite.DOColor(_damageColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);
        }

        private IEnumerator DamageOverTime(int damage, float damageInterval)
        {
            while (true)
            {
                _health -= damage;
                yield return new WaitForSeconds(damageInterval);
            }
        }

        private IEnumerator ActivateDamageOverTime(int damage, float damageInterval, float damageOverTimeLifetime)
        {
            _damageOverTimeCo = StartCoroutine(DamageOverTime(damage,damageInterval));
            yield return new WaitForSeconds(damageOverTimeLifetime);
            StopCoroutine(_damageOverTimeCo);
        }
    }
}