using UnityEngine;

namespace Player
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private float _speed = 8.0f;
        [SerializeField] private int _damageToPlayer = 15;
        [SerializeField] private float _lifeCycle = 2f;

        private bool _isEnemyLaser = false;

        private PlayerController _playerController;

        private void Start()
        {
            _playerController = PlayerController.instance;
        }

        private void Update()
        {
            MoveForward();
        }

        private void OnEnable()
        {
            Invoke(nameof(DeactivateLaser), _lifeCycle);
        }

        private void MoveForward()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.up);
        }

        private void DeactivateLaser()
        {
            gameObject.SetActive(false);
        }

        public void AssignEnemyLaser()
        {
            _isEnemyLaser = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && _isEnemyLaser == true)
            {
                if (_playerController != null)
                {
                    _playerController.Damage(_damageToPlayer);
                }
            }
        }
    }
}