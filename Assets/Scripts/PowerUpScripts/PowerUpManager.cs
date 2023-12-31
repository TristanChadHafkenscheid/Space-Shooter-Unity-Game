﻿using System.Collections.Generic;
using UnityEngine;
using Player;
using System.Collections;
using Audio;
using Managers;
using PickUps;

namespace PowerUps
{
    public class PowerUpManager : MonoBehaviour
    {
        [Header("Power Up Displays")]
        [SerializeField] private PowerUpDisplay _powerUpDisplay1;
        [SerializeField] private PowerUpDisplay _powerUpDisplay2;
        [SerializeField] private PowerUpDisplay _powerUpDisplay3;

        [Header("Power Up adjustments")]
        [SerializeField] private int _maxPowerUpLevel = 10;
        [SerializeField] private ExpMagnet _expMagnet;
        [SerializeField] private float _fireRateIncrease;
        [SerializeField] private float _speedIncrease;
        [SerializeField] private float _expMagentIncrease;
        [SerializeField] private float _healOverTime;
        [SerializeField] private int _healthAmount;
        [SerializeField] private float _bigLaserCoolDown;
        [SerializeField] private float _shieldCoolDown;
        [SerializeField] private int _maxHealthIncreaseAmount;
        [SerializeField] private float _healPercentOnMaxHealthActivate;

        [SerializeField] private List<PowerUp> _powerUps = new List<PowerUp>();

        [SerializeField] private Vector3Int _powerUpsChosenDebug;

        private PlayerController _playerController;
        private UIManager _uiManager;
        private AudioManager _audioManager;

        private void Start()
        {
            _playerController = PlayerController.instance;
            _uiManager = UIManager.instance;
            _audioManager = AudioManager.Instance;

            ResetPowerUpLevels();
        }

        public void CallPowerUp(PowerUpDisplay displayedPowerUp)
        {
            displayedPowerUp.DisplayedPowerUp.level++;

            RemovePowerUpFromList();

            _audioManager.Play("ButtonPress");

            switch (displayedPowerUp.DisplayedPowerUp.powerUpId)
            {
                case 1: //fire rate increase - fire rate starts at 0.7
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
                case 7: //increase max health and heal
                    _playerController.MaxHealth += _maxHealthIncreaseAmount;
                    _uiManager.SetMaxHealth(_playerController.MaxHealth);
                    if (_playerController.Health + (int)(_playerController.MaxHealth * _healPercentOnMaxHealthActivate/100) >= 
                        _playerController.MaxHealth)
                    {
                        _playerController.Health = _playerController.MaxHealth;
                    }
                    else
                    {
                        _playerController.Health += (int)(_playerController.MaxHealth * _healPercentOnMaxHealthActivate/100);
                    }
                    _uiManager.SetHealth(_playerController.Health);
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
            _powerUpDisplay1.UpdateDisplay(selectedPowerUps[0]);
            _powerUpDisplay2.UpdateDisplay(selectedPowerUps[1]);
            _powerUpDisplay3.UpdateDisplay(selectedPowerUps[2]);
        }

        //debugger for power up
        public void SelectPowerUpsDebug()
        {
            _powerUpDisplay1.UpdateDisplay(_powerUps[_powerUpsChosenDebug.x-1]);
            _powerUpDisplay2.UpdateDisplay(_powerUps[_powerUpsChosenDebug.y-1]);
            _powerUpDisplay3.UpdateDisplay(_powerUps[_powerUpsChosenDebug.z-1]);
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
                    _powerUps.RemoveAt(i);
            }
        }
    }
}