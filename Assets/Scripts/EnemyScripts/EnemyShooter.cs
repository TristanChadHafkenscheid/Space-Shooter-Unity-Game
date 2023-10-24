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
    }
}

