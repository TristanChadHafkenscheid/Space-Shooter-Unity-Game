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
        [SerializeField] private float _expMagentIncrease;

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
                case 1: //fire rate increase
                    _playerController.FireRate += _fireRateIncrease;
                    break;
                case 2: //speed increase
                    _playerController.Speed += _speedIncrease;
                    break;
                case 3: //exp magnet increased
                    break;
                case 4: //laser increased
                    break;
                case 5: //exp magnet increased
                    break;
                case 6: //exp magnet increased
                    break;
                default:
                    break;
            }
        }
    }
}