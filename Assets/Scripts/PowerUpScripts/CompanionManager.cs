using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Audio;

public class CompanionManager : MonoBehaviour
{
    private PlayerController _playerController;

    //[SerializeField] private PowerUpDisplay _powerUpDisplay;

    [SerializeField] private List<Companion> _allUncollectedCompanions = new List<Companion>();

    private List<Companion> _playersCompanions = new List<Companion>();
    private Companion _randomlySelectedCompanion;

    private AudioManager _audioManager;
    [SerializeField] private int _scoreToSpawnCompanion = 200;

    public int ScoreToSpawnCompanion { get => _scoreToSpawnCompanion; }

    private void Start()
    {
        _playerController = PlayerController.instance;
        _audioManager = AudioManager.Instance;
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
        //    case 3: //exp magnet increased
        //        _expMagnet.IncreaseRange(_expMagentIncrease);
        //        break;
        //    case 4: //heal over time
        //        StartCoroutine(HealOverTime());
        //        break;
        //    case 5: //big laser blast
        //        StartCoroutine(ActivateBigLaserPeriodicaly());
        //        break;
        //    case 6: //shields
        //        StartCoroutine(ActivateShieldsPeriodicaly());
        //        break;
        //    default:
        //        break;
        //}
    }

    //private void RemovePowerUpFromList()
    //{
    //    for (int i = 0; i < _powerUps.Count; i++)
    //    {
    //        if (_powerUps[i].level >= _maxPowerUpLevel)
    //        {
    //            _powerUps.RemoveAt(i);
    //        }
    //    }
    //}

    public void SpawnCollectableCompanion()
    {
        if (_allUncollectedCompanions.Count <= 0)
        {
            return;
        }
        _scoreToSpawnCompanion += _scoreToSpawnCompanion;

        int randIndex = Random.Range(0, _allUncollectedCompanions.Count - 1);
        _randomlySelectedCompanion = _allUncollectedCompanions[randIndex];

        GameObject spawnedCompanion = Instantiate(_randomlySelectedCompanion.collectableCompanion, _playerController.transform.position + new Vector3(3, 3, 0), Quaternion.identity);
        spawnedCompanion.GetComponent<SpriteRenderer>().sprite = _randomlySelectedCompanion.shipAttachmentSprite;
        StartCoroutine(DestroyCollectableCompanion(spawnedCompanion));
    }

    //display companion abilities on collect

    //remove companion ability when companion health is 0


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
