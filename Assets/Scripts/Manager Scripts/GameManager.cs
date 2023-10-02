using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    public bool isCoopMode = false;

    private GameObject _pauseMenuGameObject;
    private GameObject _touchJoystickCanvas;

    private void Start()
    {
        _pauseMenuGameObject = UIManager.instance.PauseMenuPanel;
        _touchJoystickCanvas = UIManager.instance.TouchJoystickCanvas;
    }

    public void GameOver()
    {
        _isGameOver = true;
        PauseGame(true);
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused == true)
        {
            Time.timeScale = 0;
            _pauseMenuGameObject.SetActive(true);
            _touchJoystickCanvas.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            _pauseMenuGameObject.SetActive(false);
            _touchJoystickCanvas.SetActive(true);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        PauseGame(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1); //current game scene
        PauseGame(false);
    }
}
