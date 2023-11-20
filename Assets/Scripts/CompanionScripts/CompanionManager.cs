// <header>
// Purpose:  Behaviour script that handles exp magnet functionality to move and collect exp.
// Created By  : Tristan Hafkenscheid
// Created On  : --/--/----
// Modified By : Tristan Hafkenscheid
// Modified On : 11/19/2023
// Modification Note: Added summaries to methods
// Other Notes:
// </header>

using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Managers;
using Attachments;

public class CompanionManager : MonoBehaviour
{
    [SerializeField] private List<Companion> _allUncollectedCompanions = new List<Companion>();
    [SerializeField] private int _scoreToSpawnCompanion;
    [SerializeField] private float spawnOffsetPos;
    [SerializeField] private GameObject _waterJetPrefab;
    [SerializeField] private int _waterJetDamage;

    private List<Companion> _playersCompanions = new List<Companion>();
    private Companion _randomlySelectedCompanion;
    private PlayerController _playerController;
    private CompanionPanelDisplay _companionPanelDisplay;
    private UIManager _uiManager;
    private ShipAttachmentController _shipAttachmentController;
    private int _initialScoreToSpawnCompanion;

    public int ScoreToSpawnCompanion { get => _scoreToSpawnCompanion; }
    public int WaterJetDamage { get => _waterJetDamage; }

    private void Start()
    {
        _playerController = PlayerController.instance;
        _uiManager = UIManager.instance;
        _companionPanelDisplay = _uiManager.CompanionPanel;

        _initialScoreToSpawnCompanion = _scoreToSpawnCompanion;
        _shipAttachmentController = ShipAttachmentController.instance;
    }

    /// <summary>
    /// Activates companion abilities on corresponding ship attachment.
    /// </summary>
    public void ActivateCompanion()
    {
        _playersCompanions.Add(_randomlySelectedCompanion);
        _allUncollectedCompanions.Remove(_randomlySelectedCompanion);

        switch (_randomlySelectedCompanion.companionId)
        {
            case 1: //fire rate increase - fire rate starts at 0.7
                    //_fireRateIncrease *= displayedPowerUp.DisplayedPowerUp.level;
                //_playerController.FireRate -= _fireRateIncrease;
                break;
            case 2: //speed increase
                //_playerController.Speed += _speedIncrease;
                break;
            case 3:
                //fish shoot water
                ShipAttachment fishShip = FindCompanionShip(3);
                ShootWater(fishShip.gameObject);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Spawns random companion in the scene to be collected. Activates indicator arrow.
    /// </summary>
    public void SpawnCollectableCompanion()
    {
        _scoreToSpawnCompanion += _initialScoreToSpawnCompanion;

        if (_allUncollectedCompanions.Count <= 0)
        {
            return;
        }

        int randIndex = Random.Range(0, _allUncollectedCompanions.Count - 1);
        _randomlySelectedCompanion = _allUncollectedCompanions[randIndex];

        Vector3 randomSpawn = _playerController.transform.position + new Vector3(spawnOffsetPos * (Random.Range(0, 2) * 2 - 1), spawnOffsetPos * (Random.Range(0, 2) * 2 - 1), 0);

        GameObject spawnedCompanion = Instantiate(_randomlySelectedCompanion.collectableCompanion, randomSpawn, Quaternion.identity);
        spawnedCompanion.GetComponentInChildren<SpriteRenderer>().sprite = _randomlySelectedCompanion.shipAttachmentSprite;

        // updates the display for the companion to new randomly selected companion
        _companionPanelDisplay.UpdateDisplay(_randomlySelectedCompanion);

        _uiManager.ActivateCompanionArrow(spawnedCompanion);

        StartCoroutine(DestroyCollectableCompanion(spawnedCompanion));
    }

    /// <summary>
    /// Removes companion from player companion list and adds it to uncollected list.
    /// </summary>
    /// <param name="companinonToRemove"></param>
    public void RemoveCompanion(Companion companinonToRemove)
    {
        _playersCompanions.Remove(companinonToRemove);
        _allUncollectedCompanions.Add(companinonToRemove);
    }

    /// <summary>
    /// Destroys spawned collectable companion if player does not collect it after 30 seconds
    /// </summary>
    /// <param name="companion">Currently spawned in collectable companion</param>
    /// <returns></returns>
    IEnumerator DestroyCollectableCompanion(GameObject companion)
    {
        yield return new WaitForSeconds(30f);

        _uiManager.DeactivateCompanionArrow();
        if (companion != null)
        {
            Destroy(companion);
        }
    }

    /// <summary>
    /// Shoots water on either side of the fish companion.
    /// </summary>
    /// <param name="fishShip">Fish ship attachment game object</param>
    private void ShootWater(GameObject fishShip)
    {
        Vector3 waterOffsetRight = new Vector3(0.257f, -0.003f, 0);
        GameObject waterJetRight = Instantiate(_waterJetPrefab, fishShip.transform.position, fishShip.transform.rotation);
        waterJetRight.transform.parent = fishShip.transform;
        waterJetRight.transform.localPosition = waterOffsetRight;

        Vector3 waterOffsetLeft = new Vector3(-0.257f, -0.003f, 0);
        GameObject waterJetLeft = Instantiate(_waterJetPrefab, fishShip.transform.position, fishShip.transform.rotation);
        waterJetLeft.transform.parent = fishShip.transform;
        waterJetLeft.transform.localScale = new Vector3(-waterJetLeft.transform.localScale.x, waterJetLeft.transform.localScale.y,
            waterJetLeft.transform.localScale.z);
        waterJetLeft.transform.localPosition = waterOffsetLeft;
    }

    /// <summary>
    /// Finds the ship attachment that corresponds to the companion ID.
    /// </summary>
    /// <param name="companionID"></param>
    /// <returns></returns>
    private ShipAttachment FindCompanionShip(int companionID)
    {
        int playerCompanionListIndex = 0;

        for (int i = 0; i < _shipAttachmentController.AttachmentList.Count; i++)
        {
            if (_shipAttachmentController.AttachmentList[i].AttachmentCompanion.companionId == companionID)
            {
                Debug.Log("atttachemnt companion is " + _shipAttachmentController.AttachmentList[i].AttachmentCompanion);
                playerCompanionListIndex = i;
                break;
            }
        }
        return _shipAttachmentController.AttachmentList[playerCompanionListIndex];
    }
}
