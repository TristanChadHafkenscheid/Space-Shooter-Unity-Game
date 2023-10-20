using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Managers;

public class CompanionManager : MonoBehaviour
{
    [SerializeField] private List<Companion> _allUncollectedCompanions = new List<Companion>();
    [SerializeField] private int _scoreToSpawnCompanion = 200;

    private List<Companion> _playersCompanions = new List<Companion>();
    private Companion _randomlySelectedCompanion;
    private PlayerController _playerController;
    private CompanionPanelDisplay _companionPanelDisplay;

    public int ScoreToSpawnCompanion { get => _scoreToSpawnCompanion; }

    private void Start()
    {
        _playerController = PlayerController.instance;
        _companionPanelDisplay = UIManager.instance.CompanionPanel.GetComponent<CompanionPanelDisplay>();
    }

    public void ActivateCompanion()
    {
        _playersCompanions.Add(_randomlySelectedCompanion);
        _allUncollectedCompanions.Remove(_randomlySelectedCompanion);

        //switch (displayedPowerUp.DisplayedPowerUp.powerUpId)
        //{
        //    case 1: //fire rate increase - fire rate starts at 0.7
        //            //_fireRateIncrease *= displayedPowerUp.DisplayedPowerUp.level;
        //        _playerController.FireRate -= _fireRateIncrease;
        //        break;
        //    case 2: //speed increase
        //        _playerController.Speed += _speedIncrease;
        //        break;
        //    default:
        //        break;
        //}
    }

    public void SpawnCollectableCompanion()
    {
        _scoreToSpawnCompanion += _scoreToSpawnCompanion;

        if (_allUncollectedCompanions.Count <= 0)
        {
            return;
        }

        int randIndex = Random.Range(0, _allUncollectedCompanions.Count - 1);
        _randomlySelectedCompanion = _allUncollectedCompanions[randIndex];

        Vector3 randomSpawn = _playerController.transform.position + new Vector3(5 * (Random.Range(0f, 1f) * 2 - 1), 5 * (Random.Range(0f, 1f) * 2 - 1), 0);

        GameObject spawnedCompanion = Instantiate(_randomlySelectedCompanion.collectableCompanion, randomSpawn, Quaternion.identity);
        spawnedCompanion.GetComponentInChildren<SpriteRenderer>().sprite = _randomlySelectedCompanion.shipAttachmentSprite;

        //updates the display for the companion to new randomly selected companion
        _companionPanelDisplay.UpdateDisplay(_randomlySelectedCompanion);

        StartCoroutine(DestroyCollectableCompanion(spawnedCompanion));
    }

    public void RemoveCompanion(Companion companinonToRemove)
    {
        _playersCompanions.Remove(companinonToRemove);
        _allUncollectedCompanions.Add(companinonToRemove);
    }

    //delete spawned in companion if player does not collect after 20/30 seconds
    IEnumerator DestroyCollectableCompanion(GameObject companion)
    {
        yield return new WaitForSeconds(30f);
        if (companion != null)
        {
            Destroy(companion);
        }
    }
}
