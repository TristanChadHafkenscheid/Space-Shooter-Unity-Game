using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using Audio;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int _health = 100;
        [SerializeField] private float _speed = 3.5f;

        [Header("Shooting")]
        [SerializeField] private Transform _laserBarrel;
        [SerializeField] private GameObject _bigLaser;
        [SerializeField] private float _fireRate = 0.5f;
        [SerializeField] private bool _canFire = false;

        [Header("Damage")]
        [SerializeField] private GameObject _shieldsVisualizer;
        [SerializeField] private Color _damageColour;
        [SerializeField] private ParticleSystem _damageParticles;

        [Header("Cameras")]
        [SerializeField] private GameObject _startCamera;
        [SerializeField] private GameObject _playerCamera;

        private float _canFireRate = 0.1f;

        private bool _isShieldsActive = false;
        private bool _isBigLaserActive = false;

        private SpriteRenderer _sprite;
        private Rigidbody2D _rigidBody;

        private SpawnManager _spawnManager;
        private GameManager _gameManager;
        private UIManager _uiManager;
        private AudioManager _audioManager;

        public static PlayerController instance = null;

        public SpriteRenderer Sprite
        {
            get => _sprite;
            set => _sprite = value;
        }

        public bool IsShieldsActive
        {
            get => _isShieldsActive;
        }

        public bool IsBigLaserActive
        {
            get => _isBigLaserActive;
        }

        public float FireRate
        {
            get => _fireRate;
            set => _fireRate = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        void Start()
        {
            _spawnManager = SpawnManager.instance;
            _uiManager = UIManager.instance;
            _audioManager = AudioManager.Instance;

            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite == null)
            {
                Debug.LogError("Sprite on the player is NULL");
            }

            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
            if (_gameManager == null)
            {
                Debug.LogError("Game Manager is NULL");
            }

            _rigidBody = GetComponent<Rigidbody2D>();
            if (_rigidBody == null)
            {
                Debug.LogError("Rigidbody2D on the player is NULL");
            }

            _startCamera.SetActive(false);
            _playerCamera.SetActive(true);
        }

        void Update()
        {
            if (Time.time > _canFireRate && _canFire == true)
            {
                FireLaser();
            }
        }

        public void Movement(float horizontalInput, float verticalInput)
        {
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
            _rigidBody.velocity = _speed * Time.deltaTime * direction;
        }

        private void FireLaser()
        {
            _canFireRate = Time.time + _fireRate;
            _spawnManager.SpawnPlayerLaser(_laserBarrel, transform);
            _audioManager.Play("Laser");
        }

        public void Damage(int damageTaken)
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shieldsVisualizer.SetActive(false);
                _audioManager.Stop("Shield");
                return;
            }

            _health -= damageTaken;

            _uiManager.SetHealth(_health);
            DamageVisuals();
            _audioManager.Play("Hurt");

            if (_health <= 0)
            {
                _spawnManager.OnPlayerDeath();
                _sprite.DOKill();
                gameObject.SetActive(false);
                _uiManager.CheckForBestScore();
            }
        }

        private void DamageVisuals()
        {
            _sprite.DOKill();
            _sprite.color = Color.white;
            _sprite.DOColor(_damageColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);
            _damageParticles.Play();
        }

        public void ActivateShields()
        {
            _audioManager.Play("Shield");
            _isShieldsActive = true;
            _shieldsVisualizer.SetActive(true);
        }

        public void FireBigLaser()
        {
            _isBigLaserActive = true;
            _bigLaser.SetActive(true);
            _canFire = false;
            StartCoroutine(BigLaserPowerDownRoutine());
        }

        IEnumerator BigLaserPowerDownRoutine()
        {
            yield return new WaitForSeconds(1.7f);
            _canFire = true;
            _bigLaser.SetActive(false);
            _isBigLaserActive = false;
        }

        public void AddScore(int points)
        {
            _uiManager.UpdateScore(points);
        }
    }
}