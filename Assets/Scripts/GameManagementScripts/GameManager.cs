using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        //[SerializeField] private bool _isGameOver;
        public bool isCoopMode = false;

        private GameObject _pauseMenuGameObject;
        private GameObject _touchJoystickCanvas;
        private GameObject _levelUpPanel;
        private GameObject _companionPanel;
        private AudioManager _audioManager;

        private void Start()
        {
            _pauseMenuGameObject = UIManager.instance.PauseMenuPanel;
            _touchJoystickCanvas = UIManager.instance.TouchJoystickCanvas;
            _levelUpPanel = UIManager.instance.LevelUpPanel;
            _companionPanel = UIManager.instance.CompanionPanel;
            _audioManager = AudioManager.Instance;
            _audioManager.Play("BackgroundMusic");
        }

        public void GameOver()
        {
            //_isGameOver = true;
            PauseGame(true);
        }

        public void PauseGame(bool isPaused)
        {
            _audioManager.Play("ButtonPress");
            if (isPaused == true)
            {
                Time.timeScale = 0;
                _pauseMenuGameObject.SetActive(true);
                _touchJoystickCanvas.SetActive(false);
                _audioManager.Play("PauseMusic");
                _audioManager.Pause("BackgroundMusic", true);
            }
            else
            {
                Time.timeScale = 1;
                _pauseMenuGameObject.SetActive(false);
                _touchJoystickCanvas.SetActive(true);
                _audioManager.Pause("BackgroundMusic", false);
                _audioManager.Stop("PauseMusic");
            }
        }

        public void ActivateLevelUpPanel(bool isPaused)
        {
            if (isPaused == true)
            {
                Time.timeScale = 0;
                _levelUpPanel.SetActive(true);
                _touchJoystickCanvas.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                _levelUpPanel.SetActive(false);
                _touchJoystickCanvas.SetActive(true);
            }
        }

        public void ActivateCompanionPanel(bool isPaused)
        {
            if (isPaused == true)
            {
                Time.timeScale = 0;
                _companionPanel.SetActive(true);
                _touchJoystickCanvas.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                _companionPanel.SetActive(false);
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
}