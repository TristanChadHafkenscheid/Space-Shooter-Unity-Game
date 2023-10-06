using System.Collections;
using DG.Tweening;
using Managers;
using UnityEngine;
using Audio;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 3.5f;
        [SerializeField] private float _speedMultiplier = 1.5f;
        [SerializeField] Transform _laserBarrel;
        [SerializeField] private GameObject _tripleShotPrefab;
        [SerializeField] private float _fireRate = 0.5f;
        [SerializeField] private int _health = 100;
        [SerializeField] private GameObject _bigLaser;
        [SerializeField] private GameObject _shieldsVisualizer;
        [SerializeField] private SpriteRenderer _ThrusterImg;
        [SerializeField] private float _damageRate = 0.5f;
        [SerializeField] private Color _damageColour;
        [SerializeField] private ParticleSystem _damageParticles;
        [SerializeField] private bool _canFire = false;

        private float _canFireRate = 0.1f;

        private SpawnManager _spawnManager;
        private GameManager _gameManager;
        private bool _isTripleShotActive = false;
        private bool _isShieldsActive = false;
        private UIManager _uiManager;
        private float _canTakeDamage = 0f;
        private AudioManager _audioManager;
        private SpriteRenderer _sprite;

        public static PlayerController instance = null;


        public SpriteRenderer Sprite
        {
            get => _sprite;
            set => _sprite = value;
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
            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
            _uiManager = UIManager.instance;
            _audioManager = AudioManager.Instance;

            _sprite = GetComponent<SpriteRenderer>();

            if (_sprite == null)
            {
                Debug.LogError("Sprite on the player is NULL");
            }

            if (_gameManager == null)
            {
                Debug.LogError("Game Manager is NULL");
            }
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

            //need space.world for rotating player
            GetComponent<Rigidbody2D>().velocity = _speed * Time.deltaTime * direction;
        }

        private void FireLaser()
        {
            _canFireRate = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                _spawnManager.SpawnPlayerLaser(_laserBarrel, transform);
                _audioManager.Play("Laser");
            }
        }

        public void Damage(int damageTaken)
        {
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shieldsVisualizer.SetActive(false);
                return;
            }

            //_canTakeDamage = 0 and _damageRate = 0.5 at start
            if (Time.time > _canTakeDamage)
            {
                _canTakeDamage = Time.time + _damageRate;
                _health = _health - damageTaken;

                _uiManager.SetHealth(_health);
                DamageVisuals();
                _audioManager.Play("Hurt");
            }

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

        public void TripleShotActive()
        {
            _isTripleShotActive = true;
            StartCoroutine(TripleShotPowerDownRoutine());
        }

        IEnumerator TripleShotPowerDownRoutine()
        {
            yield return new WaitForSeconds(5f);
            _isTripleShotActive = false;
        }

        public void SpeedBoostActive()
        {
            _speed *= _speedMultiplier;
            _ThrusterImg.color = new Color(0, 163, 255);
            _ThrusterImg.gameObject.transform.localPosition = new Vector3(0, -4f, 0);
            _ThrusterImg.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }

        IEnumerator SpeedBoostPowerDownRoutine()
        {
            yield return new WaitForSeconds(5f);
            _speed /= _speedMultiplier;
            _ThrusterImg.color = new Color(255, 255, 255);
            _ThrusterImg.gameObject.transform.localPosition = new Vector3(0, -3.3f, 0);
            _ThrusterImg.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public void ShieldActive(float reactivateTime)
        {
            _isShieldsActive = true;
            _shieldsVisualizer.SetActive(true);
        }

        public void BigLaserActive()
        {
            _bigLaser.SetActive(true);
            _canFire = false;
            StartCoroutine(BigLaserPowerDownRoutine());
        }

        IEnumerator BigLaserPowerDownRoutine()
        {
            yield return new WaitForSeconds(5.5f);
            _canFire = true;
            _bigLaser.SetActive(false);
        }

        public void AddScore(int points)
        {
            _uiManager.UpdateScore(points);
        }
    }
}