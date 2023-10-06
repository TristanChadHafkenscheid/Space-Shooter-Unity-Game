using System.Collections.Generic;
using UnityEngine;
using Player;
using PowerUps;
using System.Collections;

namespace Managers
{
    public class PowerUpManager : MonoBehaviour
    {
        private PlayerController _playerController;
        [SerializeField] private ExpMagnet _expMagnet;
        private UIManager _uiManager;
        [SerializeField] private float _fireRateIncrease;
        [SerializeField] private float _speedIncrease;
        [SerializeField] private float _expMagentIncrease;
        [SerializeField] private float _healOverTime;
        [SerializeField] private int _healthAmount;
        [SerializeField] private float _bigLaserCoolDownDecrease;
        [SerializeField] private float _shieldCoolDown;

        [SerializeField] private PowerUpDisplay _powerUpDisplay1;
        [SerializeField] private PowerUpDisplay _powerUpDisplay2;
        [SerializeField] private PowerUpDisplay _powerUpDisplay3;

        //[SerializeField] private float _shieldActiveAmountIncrease;

        [SerializeField] private List<PowerUp> _powerUps = new List<PowerUp>();


        private void Start()
        {
            _playerController = PlayerController.instance;
            _uiManager = UIManager.instance;
        }

        public void CallPowerUp(PowerUpDisplay displayedPowerUp)
        {
            switch (displayedPowerUp.DisplayedPowerUp.powerUpId)
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
                    StartCoroutine(HealOverTime());
                    break;
                case 5: //big laser blast
                    break;
                case 6: //shields
                    StartCoroutine(ActivateShieldsPeriodicly());
                    break;
                default:
                    break;
            }
        }

        IEnumerator HealOverTime()
        {
            while (_playerController.Health < 100)
            {
                _playerController.Health += _healthAmount;
                _uiManager.SetHealth(_playerController.Health);

                yield return new WaitForSeconds(_healOverTime);
            }
        }

        IEnumerator ActivateShieldsPeriodicly()
        {
            //while shield is false
            while (_playerController.IsShieldsActive == false)
            {
                yield return new WaitForSeconds(_shieldCoolDown);
                //activate shield
                _playerController.ActivateShields();
            }
        }

        public void RandomlySelectPowerUps()
        {
            List<PowerUp> selectedPowerUps = new List<PowerUp>();

            for (int i = 0; i < 3; i++)
            {
                int randomPowerIndex = Random.Range(0, _powerUps.Count);
                PowerUp selectedpowerUp = _powerUps[randomPowerIndex];
                if (selectedPowerUps.Contains(selectedpowerUp))
                {
                    i--;
                    continue;
                }
                selectedPowerUps.Add(selectedpowerUp);
            }
            _powerUpDisplay1.DisplayedPowerUp = selectedPowerUps[0];
            _powerUpDisplay1.UpdateDisplay();
            _powerUpDisplay2.DisplayedPowerUp = selectedPowerUps[1];
            _powerUpDisplay2.UpdateDisplay();
            _powerUpDisplay3.DisplayedPowerUp = selectedPowerUps[2];
            _powerUpDisplay3.UpdateDisplay();
        }
    }
}