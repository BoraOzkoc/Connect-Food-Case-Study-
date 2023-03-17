using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonProperties : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button levelButton;
    private LevelController _levelController;
    private int levelNumber;
    private bool _isUnlocked; 

    public void Init(int givenNumber, bool state,LevelController levelController)
    {
        _levelController = levelController;
        _isUnlocked = state;
        levelNumber = givenNumber;
        
        SetLevelText();
        SetLockState();
    }

    private void SetLevelText()
    {
        levelText.text = levelNumber.ToString();
    }

    private void SetLockState()
    {
        levelButton.interactable = _isUnlocked;

        if (!_isUnlocked)
        {
            Color tempColor = levelText.color;
            tempColor.a = 0.5f;
            levelText.color = tempColor;
        }
    }

    public void InstantiateLevel()
    {
        LevelController tempLevel = Instantiate(_levelController, Vector3.zero, Quaternion.identity);
        tempLevel.Init();
    }
}