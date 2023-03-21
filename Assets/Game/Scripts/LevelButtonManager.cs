using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private List<LevelButtonProperties> levelButtonList = new List<LevelButtonProperties>();
    [SerializeField] private List<LevelController> levelList = new List<LevelController>();
    [SerializeField] private Button playButton, nextButton;
    [SerializeField] private TextMeshProUGUI playButtonText, moveCountText;
    [SerializeField] private RayCastController rayCastController;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private List<TextMeshProUGUI> levelItemCountTextList = new List<TextMeshProUGUI>();
    [SerializeField] private List<Image> itemImageList = new List<Image>();
    private LevelButtonProperties selectedLevelButtonProperties;
    private LevelController _currentLevelController;
    private int moveCount, _currentLevel;
    private int[] levelItemCountList = new int[7];

    public void Init(int savedLevel)
    {
        DeselectButton();
        //Debug.Log("savedLevel="+savedLevel);

        if (savedLevel == 0) savedLevel = 1;
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
        // rayCastController.SubscribeToTouchEvents();

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

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }


    private void ResetItemCountList()
    {
        SetLevelItemCounts();
    }

    public void DeletePreviousLevel()
    {
        //Debug.Log("delete method called");

        if (selectedLevelButtonProperties)
        {
            ResetMoveCount();
            Destroy(selectedLevelButtonProperties.GetActiveLevel().gameObject);
            //Debug.Log("deleted");
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
        IEnumerator WaitForFail()
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
                    yield return new WaitForSeconds(0.2f);

                    gameManager.EndGame(false);
                }
            }
        }

        StartCoroutine(WaitForFail());
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
        _currentLevel = selectedLevelButtonProperties.GetLevel();
        
        if (_currentLevel >= levelButtonList.Count) nextButton.gameObject.SetActive(false);
        else nextButton.gameObject.SetActive(true);
        
        moveCount = selectedLevelButtonProperties.GetMoveCount();
        EditMoveCountText(moveCount);
        selectedLevelButtonProperties.GetSelected();
        playButton.interactable = true;
        TogglePlayButtonText(true);
    }

    public  void PrepareNextLevel()
    {
        DeletePreviousLevel();
        for (int i = 0; i < levelButtonList.Count; i++)
        {
            if (levelButtonList[i].GetLevel() == _currentLevel)
            {
                SetSelectedLevel(levelButtonList[i + 1]);
                InstantiateSelectedLevel();
                return;
            }
        }
    }

    public void CheckLevelGoals(int itemID, int amount)
    {
        for (int i = 0; i < levelItemCountList.Length; i++)
        {
            if (i == itemID)
            {
                levelItemCountList[i] -= amount;
                if (levelItemCountList[i] <= 0)
                {
                    levelItemCountList[i] = 0;
                    CloseVisuals(i);
                }
            }
        }

        UpdateCountTexts();

        CheckGoalCounts();
    }

    private void CheckGoalCounts()
    {
        IEnumerator WaitForWin()
        {
            for (int i = 0; i < levelItemCountList.Length; i++)
            {
                if (levelItemCountList[i] > 0) StopCoroutine(WaitForWin());
            }

            rayCastController.UnsubscribeFromTouchEvents();
            yield return new WaitForSeconds(0.2f);
            gameManager.EndGame(true);
        }

        StartCoroutine(WaitForWin());
        
    }

    private void SetLevelItemCounts()
    {
        OpenAllVisuals();

        int[] tempItemCountList = selectedLevelButtonProperties.GetActiveLevel().GetItemCountList();
        for (int i = 0; i < tempItemCountList.Length; i++)
        {
            int amount = tempItemCountList[i];
            levelItemCountList[i] = amount;
            if (amount <= 0) CloseVisuals(i);
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

    private void OpenAllVisuals()
    {
        for (int i = 0; i < itemImageList.Count; i++)
        {
            itemImageList[i].gameObject.SetActive(true);
        }
    }

    private void CloseVisuals(int order)
    {
        //Debug.Log("close visuals called");
        itemImageList[order].gameObject.SetActive(false);
    }

    public void EditMoveCountText(int count)
    {
        moveCountText.text = count.ToString();
    }

    public void InstantiateSelectedLevel()
    {
        IEnumerator StartLoadingScreen()
        {
            canvasManager.ActivateLoadingScreen();
            
            yield return new WaitForSeconds(Random.Range(1f,3f));
            
            Debug.Log("wait finished");
            DeselectButton();
            selectedLevelButtonProperties.GetDeselected();
            selectedLevelButtonProperties.InstantiateLevel();
            SetLevelItemCounts();
            GameManager.instance.StartLevel();
        }

        StartCoroutine(StartLoadingScreen());
        
        
    }
}