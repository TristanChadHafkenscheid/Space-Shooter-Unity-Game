using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipAttachment : MonoBehaviour
{
    public int powerupID; //0 equals triple shot, 1 = speed, 2 = shields, 3 = big laser
    public Transform botOfAttachment;

    private ShipAttachmentController _shipAttachmentController;

    private void Start()
    {
        _shipAttachmentController = ShipAttachmentController.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _shipAttachmentController.RemoveAttachment();
    }
}
