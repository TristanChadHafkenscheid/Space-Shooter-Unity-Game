using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    private UIManager _uiManager;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private float _currentExp = 0;
    [SerializeField] private float _expToFillBarFromZero = 5;
    [SerializeField] private int _expLevel = 1;

    private void Start()
    {
        _uiManager = UIManager.instance;
    }

    public void ExpCollected(int expAdded)
    {
        _currentExp += expAdded;

        if (_currentExp == _expToFillBarFromZero)
        {
            LevelUp();
        }
        else
        {
            float fillBarAmount = _currentExp / _expToFillBarFromZero * 100;
            _uiManager.SetExpBar(fillBarAmount);
        }
    }

    private void LevelUp()
    {
        Debug.Log("Leveled up!");
        _expLevel++;
        _expToFillBarFromZero *= _expLevel;

        _currentExp = 0;
        _uiManager.SetExpBar(0);
        _gameManager.ActivateLevelUpPanel(true);
    }
}
