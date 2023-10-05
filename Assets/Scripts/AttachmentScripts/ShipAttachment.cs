using System.Net.Mail;
using UnityEngine;

namespace Attachments
{
    public class ShipAttachment : MonoBehaviour
    {
        //public int powerupID; //0 equals triple shot, 1 = speed, 2 = shields, 3 = big laser
        [SerializeField] private Transform _botOfAttachment;
        [SerializeField] private int _health = 1;
        [SerializeField] private float _timeBeforeDamage = 1f;

        private HingeJoint2D _joint;
        private float _timer = 0f;

        public HingeJoint2D Joint { get => _joint; }
        public Transform BotOfAttachment { get => _botOfAttachment; }


        private ShipAttachmentController _shipAttachmentController;

        private void Awake()
        {
            _joint = GetComponent<HingeJoint2D>();
        }

        private void Start()
        {
            _shipAttachmentController = ShipAttachmentController.instance;
        }

        void Update()
        {
            _timer += Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //damage buffer of _timeBeforeDamage before taking damage again
                if (_timer <= _timeBeforeDamage)
                {
                    _timer = 0;
                    return;
                }

                _health--;

                if (_health <= 0)
                {
                    _shipAttachmentController.RemoveAttachment(this);
                }
            }
        }
    }
}