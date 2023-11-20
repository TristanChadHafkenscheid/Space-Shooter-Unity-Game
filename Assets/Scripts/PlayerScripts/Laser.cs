// <header>
// Purpose:  Behaviour script that controls laser movement and damage to enemies and player.
// Created By  : Tristan Hafkenscheid
// Created On  : --/--/----
// Modified By : Tristan Hafkenscheid
// Modified On : 11/19/2023
// Modification Note: Added summaries to methods
// Other Notes:
// </header>

using UnityEngine;

namespace Player
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private float _speed = 8.0f;
        [SerializeField] private int _damageToPlayer = 15;
        [SerializeField] private float _lifeCycle = 2f;
        [SerializeField] private int _damageToEnemy = 1;
        [SerializeField] private bool _isEnemyLaser = false;
        [SerializeField] int critChance = 10;

        private float timer = 0;
        private PlayerController _playerController;
        private int critDamageMultiplier = 2;
        private bool critActive = false;

        public static int playerDamageAdder = 0;

        /// <summary>
        /// Getter for Damage to enemy ammount and adds playerDamageAdder.
        /// Critical hit multipler is multiplied if critical hit is active.
        /// </summary>
        public int DamageToEnemy
        {
            get 
            {
                return critActive ? (_damageToEnemy + playerDamageAdder) * critDamageMultiplier 
                    : _damageToEnemy + playerDamageAdder;
            }
        }

        private void OnEnable()
        {
            if (Random.Range(0, 100) <= critChance)
            {
                critActive = true;
            }
            else
            {
                critActive = false;
            }
        }

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

        /// <summary>
        /// Moves laser in the forward direction.
        /// </summary>
        private void MoveForward()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.up);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // disable laser if it hits another enemy laser
            if (!collision.gameObject.CompareTag("EnemyLaser"))
            {
                gameObject.SetActive(false);
            }
            if (collision.gameObject.CompareTag("Player") && _isEnemyLaser == true)
            {
                _playerController.Damage(_damageToPlayer);
            }
        }
    }
}