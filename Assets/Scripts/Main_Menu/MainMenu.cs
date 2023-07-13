using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _backgroundMusic;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio source is null");
        }
        DontDestroyOnLoad(_backgroundMusic);
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
}
