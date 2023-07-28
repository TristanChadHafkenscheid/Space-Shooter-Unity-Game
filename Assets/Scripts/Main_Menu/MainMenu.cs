using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _backgroundMusic;
    [SerializeField]
    private TextMeshProUGUI _startButtonText;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio source is null");
        }
        DontDestroyOnLoad(_backgroundMusic);
        StartCoroutine(StartFlickerRoutine());
    }

    public void LoadGame()
    {
        _audioSource.Play();
        StartCoroutine(WaitForSoundBeforeSwitching());
    }

    IEnumerator WaitForSoundBeforeSwitching()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        SceneManager.LoadScene(1); //main game scene
    }

    IEnumerator StartFlickerRoutine()
    {
        while (true)
        {
            _startButtonText.text = "START GAME!!";
            yield return new WaitForSeconds(0.5f);
            _startButtonText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
