using System.Collections.Generic;
using UnityEngine;

namespace Attachments
{
    public class ShipAttachmentController : MonoBehaviour
    {
        [SerializeField] private Transform _botOfPlayer;
        [SerializeField] private List<ShipAttachment> _attachmentsList = new List<ShipAttachment>();
        [SerializeField] private int _attachmentSizeCap = 5;
        [SerializeField] private float _timeBeforeDamage = 0.5f;
        [SerializeField] private Rigidbody2D _playerRigidbody;
        private float _timer = 0f;

        public static ShipAttachmentController instance = null;

        //change this after testing
        public GameObject attachTest;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        void Update()
        {
            if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AddAttachment(attachTest);
            }

            _timer += Time.deltaTime;
        }

        //add ship attachment and instantiate it and parent to player
        public void AddAttachment(GameObject attachment)
        {
            if (_attachmentsList.Count == _attachmentSizeCap)
            {
                return;
            }

            GameObject newShipAttachmentObj;
            ShipAttachment newShipAttachment;

            if (_attachmentsList.Count == 0)
            {
                newShipAttachmentObj = Instantiate(attachment.gameObject, _botOfPlayer.position, Quaternion.identity);

                newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();

                newShipAttachment.Joint.connectedBody = _playerRigidbody;
                newShipAttachment.Joint.anchor = new Vector2(0f, 0.05f);
                newShipAttachment.Joint.connectedAnchor = new Vector2(0, -0.12f);
            }
            //last postion
            else
            {
                newShipAttachmentObj = Instantiate(attachment.gameObject,
                    _attachmentsList[_attachmentsList.Count - 1].BotOfAttachment.position, Quaternion.identity);

                newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();
                newShipAttachment.Joint.connectedBody = _attachmentsList[_attachmentsList.Count - 1].GetComponent<Rigidbody2D>();
                newShipAttachment.Joint.anchor = new Vector2(0, 0.05f);
                newShipAttachment.Joint.connectedAnchor = new Vector2(0, -0.05f);
            }
            _attachmentsList.Add(newShipAttachment);
        }

        //remove ship attachment and move the rest up 1
        public void RemoveAttachment(ShipAttachment attachment)
        {
            //damage buffer of _timeBeforeDamage before taking damage again
            if (_timer <= _timeBeforeDamage)
            {
                _timer = 0;
                return;
            }

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