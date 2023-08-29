using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplier = 1.5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _bigLaser;
    [SerializeField] private GameObject _shieldsVisualizer;
    [SerializeField] private GameObject _rightEngine, _leftEngine;
    [SerializeField] private SpriteRenderer _ThrusterImg;
    [SerializeField] private AudioClip _laserAudioClip, _laserTripleShotAudioClip;
    [SerializeField] private bool _movingToStart = false;
    [SerializeField] private float _damageRate = 0.5f;
    [SerializeField] private Color _damageColour;

    [SerializeField] private SpriteRenderer _playerSprite;
    private float _canFireRate = 0.1f;
    private bool _canFire = false;
    private SpawnManager _spawnManager;
    private GameManager _gameManager;
    private bool _isTripleShotActive = false;
    private bool _isShieldsActive = false;
    private UIManager _uiManager;
    private AudioSource _audioSource;
    private float _canTakeDamage = 0f;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("==Canvas==").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();
        //_playerSprite = GetComponent<SpriteRenderer>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

        //if (_playerSprite = null)
        //{
        //    Debug.LogError("Sprite Renderer on the player is NULL");
        //}

        StartCoroutine(MoveToStartPosition(4f));
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
        
        transform.Translate(_speed * Time.deltaTime * direction);

        //clamps max height player can go on screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 5.2f), 0);

        if (transform.position.x >= 3f)
        {
            transform.position = new Vector3(-3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -3f)
        {
            transform.position = new Vector3(3f, transform.position.y, 0);
        }
    }

    private void FireLaser()
    {
        _canFireRate = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.05f, 0), Quaternion.identity);
            _audioSource.Play();
        }
    }

    public void Damage()
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
            _lives--;
            DamageFlash();
        }

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
            _uiManager.CheckForBestScore();
        }
    }

    private void DamageFlash()
    {
        _playerSprite.DOColor(_damageColour, 0.5f).SetInverted().SetLoops(2, LoopType.Restart);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _audioSource.clip = _laserTripleShotAudioClip;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
        _audioSource.clip = _laserAudioClip;
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

    public void ShieldsActive()
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

    IEnumerator MoveToStartPosition(float lerpDuration)
    {
        _movingToStart = true;
        Vector3 offScreenPos = new Vector3(0, -6.3f, 0);
        Vector3 onScreenPos = new Vector3(0, -2f, 0);
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(offScreenPos, onScreenPos, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = onScreenPos;
        _movingToStart = false;
        _canFire = true;
    }
}