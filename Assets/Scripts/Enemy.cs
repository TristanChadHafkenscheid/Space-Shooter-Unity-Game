using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;
    private Animator _animator;
    private Collider2D _colldier;

    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on player is NULL");
        }

        _colldier = GetComponent<Collider2D>();
        if (_colldier == null)
        {
            Debug.LogError("Colldier2D on enemy is NULL");
        }
    }

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        float randX = Random.Range(-8f, 8f);
        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(randX, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            Destroy(gameObject, 2.8f);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            Destroy(gameObject, 2.8f);
        }

        if (other.CompareTag("BigLaser"))
        {
            if (_player != null)
            {
                _player.AddScore(20);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            Destroy(gameObject, 2.8f);
        }
    }
}
