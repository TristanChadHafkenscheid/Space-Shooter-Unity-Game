// <header>
// Purpose:  Behaviour script for enemy type shooter.
// Created By  : Tristan Hafkenscheid
// Created On  : --/--/----
// Modified By : Tristan Hafkenscheid
// Modified On : 11/19/2023
// Modification Note: Added summaries to methods
// Other Notes:
// </header>

using UnityEngine;

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

        protected override void Update()
        {
            base.Update();

            if (Time.time > _canFireRate && _canFire == true)
                FireLaser();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                _inPlayerVicinity = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                _inPlayerVicinity = false;
        }

        /// <summary>
        /// Fires laser based on fire rate.
        /// </summary>
        private void FireLaser()
        {
            _canFireRate = Time.time + _fireRate;
            _spawnManager.SpawnEnemyLaser(_laserBarrel, transform);
            _audioManager.Play("Laser");
        }

        /// <summary>
        /// Moves enemy towards player if they are not in collider vicinity.
        /// </summary>
        /// <param name="direction"></param>
        protected override void MoveEnemy(Vector2 direction)
        {
            if (!_inPlayerVicinity)
                base.MoveEnemy(direction);
        }
    }
}

