using System.Collections.Generic;
using UnityEngine;

namespace Attachments
{
    public class ShipAttachmentController : MonoBehaviour
    {
        [SerializeField] private Transform _botOfPlayer;
        [SerializeField] private List<ShipAttachment> _attachmentsList = new List<ShipAttachment>();
        [SerializeField] private int _attachmentSizeCap = 5;
        [SerializeField] private Rigidbody2D _playerRigidbody;
        [SerializeField] GameObject attachmentObject;
        [SerializeField] Companion attachmentCompanionTest;

        public static ShipAttachmentController instance = null;

        public List<ShipAttachment> AttachmentList
        {
            get { return _attachmentsList; }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            if (_playerRigidbody == null)
            {
                Debug.LogError("rigidbody on the player is NULL");
            }
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AddAttachment(attachmentCompanionTest);
            }
        }

        //add ship attachment and instantiate it and parent to player
        public void AddAttachment(Companion companion)
        {
            if (_attachmentsList.Count == _attachmentSizeCap)
            {
                return;
            }

            GameObject newShipAttachmentObj;
            ShipAttachment newShipAttachment;

            attachmentObject.GetComponent<SpriteRenderer>().sprite = companion.shipAttachmentSprite;

            //first position
            if (_attachmentsList.Count == 0)
            {
                newShipAttachmentObj = Instantiate(attachmentObject.gameObject, _botOfPlayer.position, 
                    Quaternion.identity);

                newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();

                newShipAttachment.Joint.connectedBody = _playerRigidbody;
                newShipAttachment.Joint.anchor = new Vector2(0f, 0.05f);
                newShipAttachment.Joint.connectedAnchor = new Vector2(0, -0.1f);
                newShipAttachment.AttachmentCompanion = companion;
            }
            //last postion
            else
            {
                newShipAttachmentObj = Instantiate(attachmentObject.gameObject,
                    _attachmentsList[_attachmentsList.Count - 1].BotOfAttachment.position, Quaternion.identity);

                newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();
                newShipAttachment.Joint.connectedBody = _attachmentsList[_attachmentsList.Count - 1].GetComponent<Rigidbody2D>();
                newShipAttachment.Joint.anchor = new Vector2(0, 0.046f);
                newShipAttachment.Joint.connectedAnchor = new Vector2(0, -0.04f);
                newShipAttachment.AttachmentCompanion = companion;
            }
            _attachmentsList.Add(newShipAttachment);
        }

        //remove ship attachment and move the rest up 1
        public void RemoveAttachment(ShipAttachment attachment)
        {
            _attachmentsList.Remove(attachment);

            Destroy(attachment.gameObject);

            for (int i = 0; i < _attachmentsList.Count; i++)
            {
                if (i == 0)
                {
                    _attachmentsList[i].Joint.connectedBody = _playerRigidbody;
                    _attachmentsList[i].Joint.anchor = new Vector2(0f, 0.05f);
                    _attachmentsList[i].Joint.connectedAnchor = new Vector2(0, -0.12f);
                }
                else
                {
                    _attachmentsList[i].Joint.connectedBody = _attachmentsList[i - 1].GetComponent<Rigidbody2D>();
                    _attachmentsList[i].Joint.anchor = new Vector2(0, 0.05f);
                    _attachmentsList[i].Joint.connectedAnchor = new Vector2(0, -0.05f);
                }
            }
        }
    }
}