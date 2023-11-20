// <header>
// Purpose:  Behaviour script that handles exp magnet functionality to move and collect exp.
// Created By  : Tristan Hafkenscheid
// Created On  : --/--/----
// Modified By : Tristan Hafkenscheid
// Modified On : 11/19/2023
// Modification Note: Added summaries to methods
// Other Notes:
// </header>

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
        [SerializeField] private float _magnetSpeed;
        [SerializeField] private float _closeToPlayer;
        [SerializeField] private ExpManager _expManager;
        [SerializeField] private ParticleSystem _expCollectParticles;
        [SerializeField] private Animator _expAura;
        [SerializeField] private Color _expPlayerColour;
        [SerializeField] private float _magnetRange = 0.2f;

        private PlayerController _playerController;
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

        /// <summary>
        /// Moves exp game object to player and collects it.
        /// </summary>
        /// <param name="expGameObject">Exp GameObject to move</param>
        /// <returns></returns>
        IEnumerator MoveExp(GameObject expGameObject)
        {
            float speedModifier = 1;
            _expAura.SetTrigger("ExpAura");

            while (Vector3.Distance(expGameObject.transform.position, transform.position) >= _closeToPlayer)
            {
                expGameObject.transform.position = Vector3.MoveTowards(expGameObject.transform.position,
                    _playerController.transform.position, _magnetSpeed * speedModifier * Time.deltaTime);
                speedModifier += 0.1f; // increases each cycle to make sure it reaches player
                yield return null;
            }

            expGameObject.SetActive(false);
            _expCollectParticles.Play();
            _audioManager.Play("ExpCollect");

            _playerController.Sprite.DOKill();
            _playerController.Sprite.color = Color.white;
            _playerController.Sprite.DOColor(_expPlayerColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);

            _expManager.ExpCollected(expGameObject.GetComponent<Exp>().ExpAmount);
        }

        /// <summary>
        /// Increases radius of exp magnets colldier.
        /// </summary>
        /// <param name="rangeIncrease">Number to be added to collider radius</param>
        public void IncreaseRange(float rangeIncrease)
        {
            _magnetCollider.radius = _magnetRange + rangeIncrease;
        }
    }
}