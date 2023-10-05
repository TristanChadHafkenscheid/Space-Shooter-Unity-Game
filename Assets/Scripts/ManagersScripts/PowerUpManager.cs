using System.Collections.Generic;
using UnityEngine;
using Player;
using PowerUps;

namespace Managers
{
    public class PowerUpManager : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private ExpMagnet _expMagnet;
        [SerializeField] private float _fireRateIncrease;
        [SerializeField] private float _speedIncrease;
        [SerializeField] private float _expMagentIncrease;
        [SerializeField] private float _healOverTime;
        [SerializeField] private float _bigLaserCoolDownDecrease;
        //[SerializeField] private float _shieldActiveAmountIncrease;

        [SerializeField] private List<PowerUp> _powerUps = new List<PowerUp>();


        private void Start()
        {
            _playerController = PlayerController.instance;
        }

        public void CallPowerUp(int _powerUpId)
        {
            switch (_powerUpId)
            {
                case 1: //fire rate increase
                    _playerController.FireRate -= _fireRateIncrease;
                    break;
                case 2: //speed increase
                    _playerController.Speed += _speedIncrease;
                    break;
                case 3: //exp magnet increased
                    _expMagnet.IncreaseRange(_expMagentIncrease);
                    break;
                case 4: //heal over time
                    break;
                case 5: //big laser blast
                    break;
                case 6: //shields
                    break;
                default:
                    break;
            }
        }
    }
}