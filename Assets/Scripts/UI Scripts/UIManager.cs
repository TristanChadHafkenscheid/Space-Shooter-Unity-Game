using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText, _highScoreText, _gameOverScoreText;
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _LevelUpPanel;
    [SerializeField] private GameObject _touchJoystickCanvas;

    public GameObject PauseMenuPanel { get { return _pauseMenuPanel; } }
    public GameObject LevelUpPanel { get { return _LevelUpPanel; } }
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
        {
            GameOverSequence();
        }
    }

    public void SetMaxHealth(int health)
    {
        _playerHealthSlider.maxValue = health;
        _playerHealthSlider.value = health;
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
}
