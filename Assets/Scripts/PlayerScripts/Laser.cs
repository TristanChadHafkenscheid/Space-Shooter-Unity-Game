﻿using UnityEngine;

namespace Player
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private float _speed = 8.0f;
        [SerializeField] private int _damageToPlayer = 15;
        [SerializeField] private float _lifeCycle = 2f;
        private float timer = 0;

        private bool _isCriticalHit = false;

        [SerializeField] private bool _isEnemyLaser = false;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController = PlayerController.instance;
        }

        private void Update()
        {
            MoveForward();

            timer += Time.deltaTime;

            if (timer >= _lifeCycle)
            {
                gameObject.SetActive(false);
                timer = 0;
            }
        }

        private void MoveForward()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.up);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("EnemyLaser"))
                gameObject.SetActive(false);

            if (collision.gameObject.CompareTag("Player") && _isEnemyLaser == true)
            {
                _playerController.Damage(_damageToPlayer);
            }
        }
    }
}