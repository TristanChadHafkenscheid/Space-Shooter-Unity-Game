using UnityEngine;
using Managers;
using Audio;
using DG.Tweening;
using UnityEngine.UI;

namespace Attachments
{
    public class ShipAttachment : MonoBehaviour
    {
        [SerializeField] private Transform _botOfAttachment;
        [SerializeField] private int _health;
        [SerializeField] private GameObject _healthBarPref;
        [SerializeField] private Color _damageColour;

        private ShipAttachmentController _shipAttachmentController;
        private CompanionManager _companionManager;
        private GameObject _healthBar;
        private SpriteRenderer _sprite;
        private AudioManager _audioManager;
        private Slider _healthSlider;
        private HingeJoint2D _joint;
        private Companion _attachmentCompanion;
        private SpawnManager _spawnManager;

        public HingeJoint2D Joint { get => _joint; }
        public Transform BotOfAttachment { get => _botOfAttachment; }
        public Companion AttachmentCompanion 
        { 
            get => _attachmentCompanion; 
            set => _attachmentCompanion = value;
        }

        private void Awake()
        {
            _joint = GetComponent<HingeJoint2D>();
        }

        private void Start()
        {
            _shipAttachmentController = ShipAttachmentController.instance;
            _companionManager = GameObject.FindGameObjectWithTag("Player").GetComponent<CompanionManager>();
            _audioManager = AudioManager.Instance;
            _spawnManager = SpawnManager.instance;

            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite == null)
            {
                Debug.LogError("Sprite on attachment is NULL");
            }

            GameObject worldSpaceCanvas = GameObject.FindGameObjectWithTag("CanvasWorldSpace");

            _healthBar = Instantiate(_healthBarPref, transform.position, Quaternion.identity, worldSpaceCanvas.transform);

            _healthSlider = _healthBar.GetComponentInChildren<Slider>();
        }

        void Update()
        {
            _healthBar.transform.SetPositionAndRotation(transform.position, transform.rotation);
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
                _companionManager.RemoveCompanion(_attachmentCompanion);
                Destroy(_healthBar);
                _spawnManager.SpawnExplosion(transform);
                _audioManager.Play("EnemyExplosion");
            }
        }

        private void DamageVisuals()
        {
            _sprite.DOKill();
            _sprite.color = Color.white;
            _sprite.DOColor(_damageColour, 0.25f).SetInverted().SetLoops(2, LoopType.Restart);
        }
    }
}