using System.Collections;
using DG.Tweening;
using Managers;
using PickUps;
using UnityEngine;

namespace Player
{
    public class ExpMagnet : MonoBehaviour
    {
        private PlayerController _playerController;

        [SerializeField] private float _magnetSpeed;
        [SerializeField] private float _closeToPlayer;
        [SerializeField] private ExpManager _expManager;
        [SerializeField] private ParticleSystem _expCollectParticles;
        [SerializeField] private Color _expPlayerColour;
        private AudioSource _audioSource;

        void Start()
        {
            _playerController = PlayerController.instance;

            _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
            {
                Debug.LogError("Audio Source on the player is NULL");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Exp"))
            {
                StartCoroutine(MoveExp(collision.gameObject));
            }
        }

        IEnumerator MoveExp(GameObject expGameObject)
        {
            while (Vector3.Distance(expGameObject.transform.position, transform.position) >= _closeToPlayer)
            {
                expGameObject.transform.position = Vector3.MoveTowards(expGameObject.transform.position,
                    _playerController.transform.position, _magnetSpeed * Time.deltaTime);
                yield return null;
            }

            //see if you can yield return a few seconds before adding to top
            expGameObject.SetActive(false);
            _expCollectParticles.Play();
            _playerController.Sprite.DOColor(_expPlayerColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);
            _audioSource.Play();

            _expManager.ExpCollected(expGameObject.GetComponent<Exp>().ExpAmount);
        }
    }
}