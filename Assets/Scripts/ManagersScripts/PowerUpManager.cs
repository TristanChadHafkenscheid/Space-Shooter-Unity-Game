using System.Collections.Generic;
using UnityEngine;
using Player;
using PowerUps;

namespace Managers
{
    public class PowerUpManager : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private float _fireRateIncrease;
        [SerializeField] private float _speedIncrease;

        [SerializeField] private List<PowerUp> _powerUps = new List<PowerUp>();

        //[SerializeField] private PowerUp _powerUp1;

        //private int _powerUpId;

        private void Start()
        {
            _playerController = PlayerController.instance;
            //_powerUpId = _powerUp1.powerUpId;
        }
        public void CallPowerUp(int _powerUpId)
        {
            switch (_powerUpId)
            {
                //fire rate increase
                case 1:
                    _playerController.FireRate += _fireRateIncrease;
                    Debug.Log("fire rate" + _playerController.FireRate);
                    break;
                case 2:
                    _playerController.Speed += _speedIncrease;
                    break;
                case 3:
                    return;
                default:
                    return;
            }
        }
    }
}