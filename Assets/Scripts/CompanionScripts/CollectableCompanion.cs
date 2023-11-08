using UnityEngine;
using Attachments;
using Managers;

public class CollectableCompanion : MonoBehaviour
{
    [SerializeField] private Companion _companion;

    private CompanionManager _companionManager;
    private ShipAttachmentController _attachmentController;
    private GameManager _gameManager;
    private UIManager _uIManager;

    void Start()
    {
        _companionManager = GameObject.FindWithTag("Player").GetComponent<CompanionManager>();
        _attachmentController = GameObject.FindWithTag("Player").GetComponent<ShipAttachmentController>();
        _gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        _uIManager = UIManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _gameManager.ActivateCompanionPanel(true);
            _uIManager.DeactivateCompanionArrow();

            _attachmentController.AddAttachment(_companion);
            _companionManager.ActivateCompanion();

            Destroy(gameObject);
        }
    }
}
