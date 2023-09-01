using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText, _bestScoreText;
    //[SerializeField] private Image _LivesImg;
    //[SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private GameObject _pauseMenuPanel;

    public GameObject PauseMenuPanel { get { return _pauseMenuPanel; } }
    public static UIManager instance = null;

    private int _score, _bestScore;
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
        _bestScore = PlayerPrefs.GetInt("HighScore", 0);
        _bestScoreText.text = "Best : " + _bestScore;
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
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
            _bestScoreText.text = "Best : " + _bestScore;
        }
    }

    //public void UpdateLives(int currentLives)
    //{
    //    Debug.Log("currentLives is:" + currentLives);

    //    _LivesImg.sprite = _liveSprites[currentLives];

    //    if (currentLives == 0)
    //    {
    //        GameOverSequence();
    //    }
    //}

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

    private void GameOverSequence()
    {
        _gameManager.GameOver();
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
