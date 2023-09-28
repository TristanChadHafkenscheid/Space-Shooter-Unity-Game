using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private int _damageToPlayer = 15;
    [SerializeField] private float _lifeCycle = 2f;

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
        Invoke(nameof(DeactivateLaser), _lifeCycle);
    }

    //private void MoveUp()
    //{
    //    transform.Translate(_speed * Time.deltaTime * Vector3.up);

    //    if (transform.position.y > 8.0f)
    //    {
    //        if (transform.parent != null)
    //        {
    //            Destroy(transform.parent.gameObject);
    //        }
    //        Destroy(this.gameObject);
    //    }
    //}

    private void MoveForward()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
    }

    private void DeactivateLaser()
    {
        gameObject.SetActive(false);
    }

    //private void MoveDown()
    //{
    //    transform.Translate(_speed * Time.deltaTime * Vector3.down);

    //    if (transform.position.y < -8.0f)
    //    {
    //        if (transform.parent != null)
    //        {
    //            Destroy(transform.parent.gameObject);
    //        }
    //        Destroy(this.gameObject);
    //    }
    //}

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isEnemyLaser == true)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(_damageToPlayer);
            }
        }
    }
}