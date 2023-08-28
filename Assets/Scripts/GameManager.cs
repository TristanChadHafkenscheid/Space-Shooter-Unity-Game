using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    public bool isCoopMode = false;

    private GameObject pauseMenuGameObject;
    private Animator _pauseAnimator;
    private Vector3 _pauseMenuStartPos;

    private void Start()
    {
        pauseMenuGameObject = UIManager.instance.PauseMenuPanel;
        _pauseMenuStartPos = pauseMenuGameObject.transform.position;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        //{
        //    SceneManager.LoadScene(1); //current game scene
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    PauseGame(true);
        //}
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseGame(bool isPaused)
    {
        if (isPaused == true)
        {
            Time.timeScale = 0;
            pauseMenuGameObject.SetActive(true);
            _pauseAnimator = UIManager.instance.PauseMenuPanel.GetComponent<Animator>();
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            _pauseAnimator.SetBool("isPaused", true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuGameObject.transform.position = _pauseMenuStartPos;
            pauseMenuGameObject.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        PauseGame(false);
    }
}
