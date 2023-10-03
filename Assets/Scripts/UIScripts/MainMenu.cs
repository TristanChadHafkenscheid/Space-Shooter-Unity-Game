using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace UIScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _backgroundMusic;
        [SerializeField] private TextMeshProUGUI _startButtonText;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("Audio source is null");
            }

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

        public void LoadCoopGame()
        {
            _audioSource.Play();
            StartCoroutine(WaitForSoundBeforeSwitchingCoop());
        }

        IEnumerator WaitForSoundBeforeSwitchingCoop()
        {
            yield return new WaitForSeconds(_audioSource.clip.length);
            SceneManager.LoadScene(2); //main game scene co-op
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
}