using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private Player _playerController;
    [SerializeField] private float _fireRateIncrease;
    [SerializeField] private float _speedIncrease;

    //[SerializeField] private PowerUp _powerUp1;

    //private int _powerUpId;

    private void Start()
    {
        _playerController = Player.instance;
        //_powerUpId = _powerUp1.powerUpId;
    }
    public void CallPowerUp(int _powerUpId)
    {
        switch (_powerUpId)
        {
            //fire rate increase
            case 1:
                _playerController.FireRate += _fireRateIncrease;
                Debug.Log("fire rate" + _playerController.FireRate);
                break;
            case 2:
                _playerController.Speed += _speedIncrease;
                break;
            case 3:
                return;
            default:
                return;
        }
    }
}
