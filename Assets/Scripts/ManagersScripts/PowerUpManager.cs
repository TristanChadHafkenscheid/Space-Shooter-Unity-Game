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
        [SerializeField] private float _fireRateIncrease;
        [SerializeField] private float _speedIncrease;
        [SerializeField] private float _expMagentIncrease;
        [SerializeField] private float _healOverTime;
        [SerializeField] private int _healthAmount;
        [SerializeField] private float _bigLaserCoolDown;
        [SerializeField] private float _shieldCoolDown;

        [SerializeField] private PowerUpDisplay _powerUpDisplay1;
        [SerializeField] private PowerUpDisplay _powerUpDisplay2;
        [SerializeField] private PowerUpDisplay _powerUpDisplay3;

        [SerializeField] private List<PowerUp> _powerUps = new List<PowerUp>();

        [SerializeField] private float _incremenetNumber;

        [SerializeField] private Vector3Int _powerUpsChosenDebug;

        private UIManager _uiManager;

        [SerializeField] private int _maxPowerUpLevel = 10;


        private void Start()
        {
            _playerController = PlayerController.instance;
            _uiManager = UIManager.instance;

            ResetPowerUpLevels();
        }

        public void CallPowerUp(PowerUpDisplay displayedPowerUp)
        {
            displayedPowerUp.DisplayedPowerUp.level++;

            RemovePowerUpFromList();

            switch (displayedPowerUp.DisplayedPowerUp.powerUpId)
            {
                case 1: //fire rate increase - fire rate starts at 0.7
                    //_fireRateIncrease *= displayedPowerUp.DisplayedPowerUp.level;
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
                    StartCoroutine(ActivateBigLaserPeriodicaly());
                    break;
                case 6: //shields
                    StartCoroutine(ActivateShieldsPeriodicaly());
                    break;
                default:
                    break;
            }
        }

        IEnumerator HealOverTime()
        {
            while (true)
            {
                if (_playerController.Health < 100)
                {
                    _playerController.Health += _healthAmount;
                    _uiManager.SetHealth(_playerController.Health);

                    yield return new WaitForSeconds(_healOverTime);
                }
                else
                    yield return null;
            }
        }

        IEnumerator ActivateShieldsPeriodicaly()
        {
            while (true)
            {
                if (_playerController.IsShieldsActive == false)
                {
                    _playerController.ActivateShields();
                    yield return new WaitForSeconds(_shieldCoolDown);
                }
                else
                    yield return null;
            }
        }

        IEnumerator ActivateBigLaserPeriodicaly()
        {
            while (true)
            {
                if (_playerController.IsBigLaserActive == false)
                {
                    yield return new WaitForSeconds(_bigLaserCoolDown);
                    _playerController.FireBigLaser();
                }
                else
                    yield return null;
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

        //debugger for power up
        public void SelectPowerUpsDebug()
        {
            _powerUpDisplay1.DisplayedPowerUp = _powerUps[_powerUpsChosenDebug.x];
            _powerUpDisplay1.UpdateDisplay();
            _powerUpDisplay2.DisplayedPowerUp = _powerUps[_powerUpsChosenDebug.y];
            _powerUpDisplay2.UpdateDisplay();
            _powerUpDisplay3.DisplayedPowerUp = _powerUps[_powerUpsChosenDebug.z];
            _powerUpDisplay3.UpdateDisplay();
        }

        private void ResetPowerUpLevels()
        {
            for (int i = 0; i < _powerUps.Count; i++)
            {
                _powerUps[i].level = 0;
            }
        }

        private void RemovePowerUpFromList()
        {
            for (int i = 0; i < _powerUps.Count; i++)
            {
                if (_powerUps[i].level >= _maxPowerUpLevel)
                {
                    _powerUps.RemoveAt(i);
                }
            }
        }
    }
}