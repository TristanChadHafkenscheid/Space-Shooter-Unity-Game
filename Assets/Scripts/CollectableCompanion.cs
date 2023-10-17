using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attachments;

public class CollectableCompanion : MonoBehaviour
{
    private CompanionManager _companionManager;
    private ShipAttachmentController _attachmentController;

    [SerializeField] private Companion _companion;

    void Start()
    {
        _companionManager = GameObject.FindWithTag("Player").GetComponent<CompanionManager>();
        _attachmentController = GameObject.FindWithTag("Player").GetComponent<ShipAttachmentController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //start companion ability
            _companionManager.ActivateCompanion();
            _attachmentController.AddAttachment(_companion.shipAttachmentSprite);

            Destroy(gameObject);
        }
    }
}
