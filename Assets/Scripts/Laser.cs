using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private int _damageToPlayer = 15;

    private bool _isEnemyLaser = false;

    private Player _playerController;

    private void Start()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        MoveForward();
        if (_isEnemyLaser == false)
        {
            //MoveUp();
            MoveForward();
        }
        else
        {
            //MoveDown();
        }
    }
    private void OnEnable()
    {
        Invoke(nameof(DeactivateLaser), 1f);
    }

    private void MoveUp()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);

        if (transform.position.y > 8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    private void MoveForward()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
    }

    private void DeactivateLaser()
    {
        gameObject.SetActive(false);
    }

    private void MoveDown()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        if (transform.position.y < -8.0f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(_damageToPlayer);
            }
        }
    }
}