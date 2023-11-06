using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Audio;

namespace Enemy
{
    public class EnemyShooter : Enemy
    {
        [Header("Shooting")]
        [SerializeField] private Transform _laserBarrel;
        [SerializeField] private float _fireRate = 0.5f;
        [SerializeField] private bool _canFire = false;
        private float _canFireRate = 0.1f;
        private bool _inPlayerVicinity = false;

        private void FireLaser()
        {
            _canFireRate = Time.time + _fireRate;
            _spawnManager.SpawnEnemyLaser(_laserBarrel, transform);
            _audioManager.Play("Laser");
        }

        protected override void Update()
        {
            base.Update();

            if (Time.time > _canFireRate && _canFire == true)
            {
                FireLaser();
            }
        }

        protected override void MoveEnemy(Vector2 direction)
        {
            if (!_inPlayerVicinity)
            {
                base.MoveEnemy(direction);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _inPlayerVicinity = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _inPlayerVicinity = false;
            }
        }
    }
}

