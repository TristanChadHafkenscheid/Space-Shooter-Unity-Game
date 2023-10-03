using UnityEngine;

namespace Attachments
{
    public class ShipAttachment : MonoBehaviour
    {
        //public int powerupID; //0 equals triple shot, 1 = speed, 2 = shields, 3 = big laser
        [SerializeField] private Transform _botOfAttachment;
        [SerializeField] private int _health = 1;

        private HingeJoint2D _joint;

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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _health--;

                if (_health <= 0)
                {
                    _shipAttachmentController.RemoveAttachment(this);
                }
            }
        }
    }
}