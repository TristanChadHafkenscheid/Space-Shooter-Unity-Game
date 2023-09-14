using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttachmentController : MonoBehaviour
{
    [SerializeField] private Transform _botOfPlayer;
    [SerializeField] private List<ShipAttachment> _attachmentsList = new List<ShipAttachment>();

    public GameObject attachTest;

    void Start()
    {

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
        ShipAttachment newShipAttachment;
        if (_attachmentsList.Count == 0)
        {
            GameObject newShipAttachmentObj = Instantiate(attachment.gameObject, _botOfPlayer.position, Quaternion.identity);
            newShipAttachmentObj.transform.parent = this.transform;

            newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();
        }
        //last postion
        else
        {
            GameObject newShipAttachmentObj = Instantiate(attachment.gameObject,
                _attachmentsList[_attachmentsList.Count].botOfAttachment.position, Quaternion.identity);
            newShipAttachmentObj.transform.parent = this.transform;

            newShipAttachment = newShipAttachmentObj.GetComponent<ShipAttachment>();
        }
        _attachmentsList.Add(newShipAttachment);
    }

    //remove ship attachment and move the rest up 1

    //rearrange ship attachment to players liking
}
