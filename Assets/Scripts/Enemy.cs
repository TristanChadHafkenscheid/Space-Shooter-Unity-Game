using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private int _damageToPlayer = 15;
    [SerializeField] private GameObject _expPref;

    private Player _player;
    private Animator _animator;
    private Collider2D _colldier;
    private AudioSource _audioSource;
    private float _fireRate = 3f;
    private float _canFire = -1f;
    private bool _isShooting = false;
    private Rigidbody2D _rigidBody;
    private Vector2 _movement;

    void Start()
    {
        _player = Player.instance;

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

        _rigidBody = GetComponent<Rigidbody2D>();

        if (_rigidBody == null)
        {
            Debug.LogError("Rigidbody on player is NULL");
        }

        _isShooting = true;
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
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage(_damageToPlayer);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            _isShooting = false;
            //Destroy(gameObject, 2.8f);
            Invoke(nameof(DisableEnemy), 2.8f);
        }

        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            collision.gameObject.SetActive(false);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            _isShooting = false;

            Invoke(nameof(SpawnExp), 0.5f);

            //Destroy(gameObject, 2.8f);
            Invoke(nameof(DisableEnemy), 2.8f);
        }

        if (collision.gameObject.CompareTag("Attachment"))
        {
            //collision.gameObject.SetActive(false);

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            _isShooting = false;
            //Destroy(gameObject, 2.8f);
            Invoke(nameof(DisableEnemy), 2.8f);
        }

        if (collision.gameObject.CompareTag("BigLaser"))
        {
            if (_player != null)
            {
                _player.AddScore(20);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();

            _colldier.enabled = false;
            _isShooting = false;
            //Destroy(gameObject, 2.8f);
            Invoke(nameof(DisableEnemy), 2.8f);
        }
    }

    private void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    private void SpawnExp()
    {
        //please pool this instead
        Instantiate(_expPref, transform.position, Quaternion.identity);
    }
}
