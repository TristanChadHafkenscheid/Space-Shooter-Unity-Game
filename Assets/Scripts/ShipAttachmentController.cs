using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttachmentController : MonoBehaviour
{
    [SerializeField] private Transform _botOfPlayer;
    [SerializeField] private List<ShipAttachment> _attachmentsList = new List<ShipAttachment>();
    [SerializeField] private int _attachmentSizeCap = 5;

    public GameObject attachTest;

    public static ShipAttachmentController instance = null;

    [SerializeField] private float _damageRate = 0.5f;
    private float _canTakeDamage = 0f;
    [SerializeField] private Rigidbody2D _playerRigidbody;

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
    }

    //rotate ship atachment around player
    private void RotateAttachment()
    {
        //_attachmentsList[0].attachmentObject.transform.ro
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
            newShipAttachmentObj.transform.parent = this.transform;

            newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();
            newShipAttachment._spring.connectedBody = _playerRigidbody;
        }
        //last postion
        else
        {
            newShipAttachmentObj = Instantiate(attachment.gameObject,
                _attachmentsList[_attachmentsList.Count - 1].botOfAttachment.position, Quaternion.identity);
            newShipAttachmentObj.transform.parent = this.transform;

            newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();
            newShipAttachment._spring.connectedBody = _attachmentsList[_attachmentsList.Count - 1].GetComponent<Rigidbody2D>();
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
                _attachmentsList[i].gameObject.transform.position = _botOfPlayer.position;
            }
            else
            {
                _attachmentsList[i].gameObject.transform.position =
                    _attachmentsList[i - 1].botOfAttachment.position;
            }
        }

        if (Time.time > _canTakeDamage)
        {
            _canTakeDamage = Time.time + _damageRate;

            //_attachmentsList.Remove(attachment);

            //Destroy(attachment.gameObject);

            //for (int i = 0; i < _attachmentsList.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        _attachmentsList[i].gameObject.transform.position = _botOfPlayer.position;
            //    }
            //    else
            //    {
            //        _attachmentsList[i].gameObject.transform.position =
            //            _attachmentsList[i - 1].botOfAttachment.position;
            //    }
            //}
        }
    }

    //rearrange ship attachment to players liking
}