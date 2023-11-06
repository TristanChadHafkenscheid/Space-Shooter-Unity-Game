using System.Collections;
using DG.Tweening;
using Managers;
using Player;
using UnityEngine;
using Audio;

namespace PickUps
{
    public class ExpMagnet : MonoBehaviour
    {
        private PlayerController _playerController;

        [SerializeField] private float _magnetSpeed;
        [SerializeField] private float _closeToPlayer;
        [SerializeField] private ExpManager _expManager;
        [SerializeField] private ParticleSystem _expCollectParticles;
        [SerializeField] private Animator _expAura;
        [SerializeField] private Color _expPlayerColour;
        [SerializeField] private float _magnetRange = 0.2f;

        private AudioManager _audioManager;

        private CircleCollider2D _magnetCollider;

        void Start()
        {
            _playerController = PlayerController.instance;

            _audioManager = AudioManager.Instance;

            _magnetCollider = GetComponent<CircleCollider2D>();
            if (_magnetCollider == null)
            {
                Debug.LogError("Collider on ExpMagnet is NULL");
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
            _expAura.SetTrigger("ExpAura");
            while (Vector3.Distance(expGameObject.transform.position, transform.position) >= _closeToPlayer)
            {
                expGameObject.transform.position = Vector3.MoveTowards(expGameObject.transform.position,
                    _playerController.transform.position, _magnetSpeed * Time.deltaTime);
                yield return null;
            }

            //see if you can yield return a few seconds before adding to top
            expGameObject.SetActive(false);
            _expCollectParticles.Play();
            _audioManager.Play("ExpCollect");

            _playerController.Sprite.DOKill();
            _playerController.Sprite.color = Color.white;
            _playerController.Sprite.DOColor(_expPlayerColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);

            _expManager.ExpCollected(expGameObject.GetComponent<Exp>().ExpAmount);
        }

        public void IncreaseRange(float rangeIncrease)
        {
            //range starts at 0.2
            _magnetCollider.radius = _magnetRange + rangeIncrease;
        }
    }
}