using UnityEngine;
using Attachments;
using Managers;

public class CollectableCompanion : MonoBehaviour
{
    private CompanionManager _companionManager;
    private ShipAttachmentController _attachmentController;
    private GameManager _gameManager;

    [SerializeField] private Companion _companion;

    void Start()
    {
        _companionManager = GameObject.FindWithTag("Player").GetComponent<CompanionManager>();
        _attachmentController = GameObject.FindWithTag("Player").GetComponent<ShipAttachmentController>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //start companion ability
            _gameManager.ActivateCompanionPanel(true);
            _companionManager.ActivateCompanion();
            _attachmentController.AddAttachment(_companion);

            Destroy(gameObject);
        }
    }
}
