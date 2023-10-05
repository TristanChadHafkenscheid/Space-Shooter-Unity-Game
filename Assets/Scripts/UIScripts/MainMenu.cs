using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Audio;

namespace UIScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _startButtonText;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager.Play("BackgroundMusic");

            StartCoroutine(StartFlickerRoutine());
        }

        public void LoadGame()
        {
            _audioManager.Play("Laser");
            StartCoroutine(WaitForSoundBeforeSwitching());
        }

        IEnumerator WaitForSoundBeforeSwitching()
        {
            yield return new WaitForSeconds(_audioManager.GetSound("Laser").clip.length);
            SceneManager.LoadScene(1); //main game scene
        }

        public void LoadCoopGame()
        {
            _audioManager.Play("Laser");
            StartCoroutine(WaitForSoundBeforeSwitchingCoop());
        }

        IEnumerator WaitForSoundBeforeSwitchingCoop()
        {
            yield return new WaitForSeconds(_audioManager.GetSound("Laser").clip.length);
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