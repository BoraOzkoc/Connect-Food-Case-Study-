using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonProperties : MonoBehaviour
{
    [SerializeField] private LevelController levelPrefab;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button levelButton;
    [SerializeField] private GameObject selectionFrame;
    private LevelController activeLevel;
    private LevelController _levelController;
    private int levelNumber;
    private bool _isUnlocked;
    private LevelButtonManager _levelButtonManager;

    public void Init(int givenNumber, bool state,LevelController levelController,LevelButtonManager levelButtonManager)
    {
        //Debug.Log("button=" +gameObject.name +"= state=="+state);
        _levelButtonManager = levelButtonManager;
        _levelController = levelController;
        _isUnlocked = state;
        levelNumber = givenNumber;
        
        SetLevelText();
        SetLockState();
        GetDeselected();
    }


    public LevelController GetActiveLevel()
    {
        return activeLevel;
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
        else
        {
            Color tempColor = levelText.color;
            tempColor.a = 1f;
            levelText.color = tempColor;
        }
    }

    public void SelectLevel()
    {
        _levelButtonManager.SetSelectedLevel(this);
    }

    public void GetSelected()
    {
        selectionFrame.SetActive(true);
    }

    public int GetLevel()
    {
        return levelNumber;
    }

    public int GetMoveCount()
    {
        return _levelController.GetMoveCount();
    }
    public void GetDeselected()
    {
        selectionFrame.SetActive(false);

    }
    public void InstantiateLevel()
    {
        LevelController tempLevel = Instantiate(_levelController, Vector3.zero, Quaternion.identity);
        activeLevel = tempLevel;
        tempLevel.Init();
    }
}