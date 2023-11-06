using UnityEngine;
using UnityEngine.InputSystem;
using PowerUps;

namespace Managers
{
    public class ExpManager : MonoBehaviour
    {
        private UIManager _uiManager;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private float _currentExp = 0;
        [SerializeField] private float _expToFillBarFromZero = 5;
        [SerializeField] private int _expLevel = 1;
        [SerializeField] private PowerUpManager _powerUpManager;

        private void Start()
        {
            _uiManager = UIManager.instance;
        }

        private void Update()
        {
            if (Keyboard.current.aKey.wasPressedThisFrame)
                LevelUpDebugger();
        }

        public void ExpCollected(int expAdded)
        {
            _currentExp += expAdded;

            if (_currentExp == _expToFillBarFromZero)
                LevelUp();
            else
            {
                float fillBarAmount = _currentExp / _expToFillBarFromZero * 100;
                _uiManager.SetExpBar(fillBarAmount);
            }
        }

        private void LevelUp()
        {
            _expLevel++;
            _expToFillBarFromZero *= _expLevel;

            _currentExp = 0;
            _uiManager.SetExpBar(0);
            _gameManager.ActivateLevelUpPanel(true);
            _powerUpManager.RandomlySelectPowerUps();
        }

        private void LevelUpDebugger()
        {
            _expLevel++;
            _expToFillBarFromZero *= _expLevel;

            _currentExp = 0;
            _uiManager.SetExpBar(0);
            _gameManager.ActivateLevelUpPanel(true);
            _powerUpManager.SelectPowerUpsDebug();
        }
    }
}