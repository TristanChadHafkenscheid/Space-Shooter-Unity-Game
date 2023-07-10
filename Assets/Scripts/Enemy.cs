using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
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
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(gameObject);
        }
    }
}
