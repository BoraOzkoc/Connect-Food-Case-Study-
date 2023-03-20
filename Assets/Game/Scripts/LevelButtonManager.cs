using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private List<LevelButtonProperties> levelButtonList = new List<LevelButtonProperties>();
    [SerializeField] private List<LevelController> levelList = new List<LevelController>();
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI playButtonText, moveCountText;
    [SerializeField] private RayCastController rayCastController;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<TextMeshProUGUI> levelItemCountTextList = new List<TextMeshProUGUI>();
    private LevelButtonProperties selectedLevelButtonProperties;
    private int moveCount, currentLevel;
    private int[] levelItemCountList = new int[7];

    public void Init(int savedLevel = 0)
    {
        DeselectButton();

        for (int i = 0; i < levelButtonList.Count; i++)
        {
            bool state = savedLevel > i;

            levelButtonList[i].Init(i + 1, state, levelList[i], this);
        }
    }

    public void Reset()
    {
        ResetMoveCount();
        ResetItemCountList();
        // Reactivate Raycast(subscribe to touch events)!
        //rayCastController.SubscribeToTouchEvents();
        
        LevelButtonProperties tempLevel = selectedLevelButtonProperties;
        DeletePreviousLevel();
        selectedLevelButtonProperties = tempLevel;
        tempLevel.InstantiateLevel();
        canvasManager.ActivateCanvas(CanvasManager.PanelType.game);
    }

    private void ResetMoveCount()
    {
        moveCount = selectedLevelButtonProperties.GetMoveCount();
        EditMoveCountText(moveCount);
    }

    private void ResetItemCountList()
    {
        SetLevelItemCounts();
    }

    public void DeletePreviousLevel()
    {
        Debug.Log("delete method called");

        if (selectedLevelButtonProperties)
        {
            ResetMoveCount();
            Destroy(selectedLevelButtonProperties.GetActiveLevel().gameObject);
            Debug.Log("deleted");
        }

        rayCastController.SubscribeToTouchEvents();
    }

    private void DeselectButton()
    {
        playButton.interactable = false;
        TogglePlayButtonText(false);
    }

    public void DecreaseCount()
    {
        if (moveCount > 0)
        {
            moveCount--;
            EditMoveCountText(moveCount);

            if (moveCount <= 0)
            {
                Debug.Log("no moves left");
                rayCastController.UnsubscribeFromTouchEvents();
                // lose condition (fail canvas)
                gameManager.EndGame(false);
            }
        }
    }

    private void TogglePlayButtonText(bool state)
    {
        if (state)
        {
            Color tempColor = playButtonText.color;
            tempColor.a = 1f;
            playButtonText.color = tempColor;
        }
        else
        {
            Color tempColor = playButtonText.color;
            tempColor.a = 0.5f;
            playButtonText.color = tempColor;
        }
    }

    public void SetSelectedLevel(LevelButtonProperties tempLevel)
    {
        if (selectedLevelButtonProperties && selectedLevelButtonProperties != tempLevel)
            selectedLevelButtonProperties.GetDeselected();
        selectedLevelButtonProperties = tempLevel;
        currentLevel = selectedLevelButtonProperties.GetLevel();
        moveCount = selectedLevelButtonProperties.GetMoveCount();
        EditMoveCountText(moveCount);
        selectedLevelButtonProperties.GetSelected();
        playButton.interactable = true;
        TogglePlayButtonText(true);
    }

    public void CheckLevelGoals(int itemID, int amount)
    {
        for (int i = 0; i < levelItemCountList.Length; i++)
        {
            if (i == itemID)
            {
                levelItemCountList[i] -= amount;
                if (levelItemCountList[i] <= 0) levelItemCountList[i] = 0;
            }
        }

        UpdateCountTexts();
        
        CheckGoalCounts();
    }

    private void CheckGoalCounts()
    {
        for (int i = 0; i < levelItemCountList.Length; i++)
        {
            if (levelItemCountList[i] > 0) return;
        }
        gameManager.EndGame(true);

    }

    private void SetLevelItemCounts()
    {
        int[] tempItemCountList = selectedLevelButtonProperties.GetActiveLevel().GetItemCountList();
        for (int i = 0; i <tempItemCountList.Length ; i++)
        {
            levelItemCountList[i] =  tempItemCountList[i];
        }

        UpdateCountTexts();
    }
    private void UpdateCountTexts()
    {
        for (int i = 0; i < levelItemCountTextList.Count; i++)
        {
            levelItemCountTextList[i].text = levelItemCountList[i].ToString();
        }
    }

    public void EditMoveCountText(int count)
    {
        moveCountText.text = count.ToString();
    }

    public void InstantiateSelectedLevel()
    {
        
        DeselectButton();
        selectedLevelButtonProperties.GetDeselected();
        selectedLevelButtonProperties.InstantiateLevel();
        SetLevelItemCounts();
        GameManager.instance.StartLevel();
    }
}