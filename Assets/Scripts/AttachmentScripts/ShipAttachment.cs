using UnityEngine;
using Managers;
using Audio;
using DG.Tweening;
using UnityEngine.UI;

namespace Attachments
{
    public class ShipAttachment : MonoBehaviour
    {
        //public int powerupID; //0 equals triple shot, 1 = speed, 2 = shields, 3 = big laser
        [SerializeField] private Transform _botOfAttachment;
        [SerializeField] private int _health = 1;
        [SerializeField] private float _timeBeforeDamage = 1f;

        [SerializeField] private GameObject _healthBarPref;

        [SerializeField] private Color _damageColour;

        private HingeJoint2D _joint;

        public HingeJoint2D Joint { get => _joint; }
        public Transform BotOfAttachment { get => _botOfAttachment; }

        private ShipAttachmentController _shipAttachmentController;
        private GameObject _healthBar;
        private Vector3 _healthBarPosOffset;
        private SpriteRenderer _sprite;
        private AudioManager _audioManager;

        private Slider _healthSlider;

        private void Awake()
        {
            _joint = GetComponent<HingeJoint2D>();
        }

        private void Start()
        {
            _shipAttachmentController = ShipAttachmentController.instance;
            _audioManager = AudioManager.Instance;

            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite == null)
            {
                Debug.LogError("Sprite on attachment is NULL");
            }

            GameObject worldSpaceCanvas = GameObject.FindGameObjectWithTag("CanvasWorldSpace");

            _healthBarPosOffset = new Vector3(0.3f, 0.2f, 0);
            _healthBar = Instantiate(_healthBarPref, transform.position + _healthBarPosOffset, Quaternion.identity, worldSpaceCanvas.transform);

            _healthSlider = _healthBar.GetComponent<Slider>();
        }

        void Update()
        {
            //_healthBar.transform.rotation = transform.rotation;
            //_healthBar.transform.position = transform.position + _healthBarPosOffset;

            _healthBar.transform.SetPositionAndRotation(transform.position + _healthBarPosOffset, transform.rotation);
        }

        public void Damage(int damageTaken)
        {
            _health -= damageTaken;

            _healthSlider.value = _health;
            DamageVisuals();

            //change this to attachment hurt sound
            _audioManager.Play("Hurt");

            if (_health <= 0)
            {
                _sprite.DOKill();
                _shipAttachmentController.RemoveAttachment(this);
                Destroy(_healthBar);
            }
        }

        private void DamageVisuals()
        {
            _sprite.DOKill();
            _sprite.color = Color.white;
            _sprite.DOColor(_damageColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);
            //_damageParticles.Play();
        }
    }
}