﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 1.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = 0.1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    //private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;

    //private bool _isBigLaserActive = false;

    [SerializeField]
    private GameObject _bigLaser;

    [SerializeField]
    private GameObject _shieldsVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private SpriteRenderer _ThrusterImg;

    [SerializeField]
    private int _score;
    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserAudioClip, _laserTripleShotAudioClip;

    private AudioSource _audioSource;

    [SerializeField]
    private bool _movingToStart = false;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("==Canvas==").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

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

        StartCoroutine(MoveToStartPosition(4f));
    }

    void Update()
    {
        if (_movingToStart == true)
        {
            return;
        }
        Movement();

#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            FireLaser();
        }
#elif UNITY_IOS
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0) && Time.time > _canFire)
        {
            FireLaser();
        }
#else
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0) && Time.time > _canFire)
        {
            FireLaser();
        }
#endif
    }

    private void Movement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); //Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical");  //Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        transform.Translate(_speed * Time.deltaTime * direction);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    //shoots laser
    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;

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

        _lives--;

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
        }
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
        //_isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        _ThrusterImg.color = new Color(0, 163, 255);
        _ThrusterImg.gameObject.transform.localPosition = new Vector3(0, -4f, 0);
        _ThrusterImg.gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        //_isSpeedBoostActive = false;
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
        //_isBigLaserActive = true;
        _bigLaser.SetActive(true);
        StartCoroutine(BigLaserPowerDownRoutine());
    }

    IEnumerator BigLaserPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.5f);
        //_isBigLaserActive = false;
        _bigLaser.SetActive(false);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator MoveToStartPosition(float lerpDuration)
    {
        _movingToStart = true;
        Vector3 offScreenPos = new Vector3(0, -7f, 0);
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
    }
}