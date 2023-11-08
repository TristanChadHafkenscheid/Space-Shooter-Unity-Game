using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText, _highScoreText, _gameOverScoreText;
        [SerializeField] private Slider _playerHealthSlider;
        [SerializeField] private Slider _expSlider;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _gameOverText;
        [SerializeField] private GameObject _pauseMenuPanel;
        [SerializeField] private GameObject _levelUpPanel;
        [SerializeField] private GameObject _touchJoystickCanvas;
        [SerializeField] private CompanionManager _companionManager;
        [SerializeField] private CompanionPanelDisplay _companionPanel;
        [SerializeField] private WindowCompanionPointer _companionArrowScreen;

        public GameObject PauseMenuPanel { get { return _pauseMenuPanel; } }
        public GameObject LevelUpPanel { get { return _levelUpPanel; } }
        public CompanionPanelDisplay CompanionPanel { get { return _companionPanel; } }
        public GameObject TouchJoystickCanvas { get { return _touchJoystickCanvas; } }
        public static UIManager instance = null;

        private int _score, _highScore;
        private GameManager _gameManager;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        void Start()
        {
            _scoreText.text = "Score: " + 0;
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
            _gameOverText.gameObject.SetActive(false);
            _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

            if (_gameManager == null)
            {
                Debug.LogError("GameManager is NULL");
            }
        }

        public void UpdateScore(int addToScore)
        {
            _score += addToScore;
            _scoreText.text = "Score: " + _score.ToString();

            if (_score >= _companionManager.ScoreToSpawnCompanion)
            {
                _companionManager.SpawnCollectableCompanion();
            }
        }

        public void CheckForBestScore()
        {
            if (_score > _highScore)
            {
                _highScore = _score;
                PlayerPrefs.SetInt("HighScore", _highScore);
            }
        }

        public void SetHealth(int playerHealth)
        {
            _playerHealthSlider.value = playerHealth;
            if (playerHealth <= 0)
                GameOverSequence();
        }

        public void SetMaxHealth(int maxHealth)
        {
            _playerHealthSlider.maxValue = maxHealth;
        }

        public void SetExpBar(float expAmount)
        {
            _expSlider.value = expAmount;
        }

        private void GameOverSequence()
        {
            _gameManager.GameOver();
            _gameOverScoreText.text = "Score: " + _score;
            _highScoreText.text = "High Score: " + _highScore;
            _gameOverPanel.SetActive(true);
            _gameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
        }

        IEnumerator GameOverFlickerRoutine()
        {
            while (true)
            {
                _gameOverText.text = "GAME OVER";
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void ActivateCompanionArrow(GameObject targetObject)
        {
            _companionArrowScreen.gameObject.SetActive(true);
            _companionArrowScreen.TargetObject = targetObject;
        }

        public void DeactivateCompanionArrow()
        {
            _companionArrowScreen.gameObject.SetActive(false);
            _companionArrowScreen.TargetObject = null;
        }
    }
}