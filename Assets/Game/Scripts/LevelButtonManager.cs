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
    private LevelButtonProperties selectedLevelButtonProperties;
    private int moveCount, currentLevel;

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
        // Reactivate Raycast(subscribe to touch events)!
        //rayCastController.SubscribeToTouchEvents();
        Debug.Log("reset called");
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
    public void DeletePreviousLevel()
    {
        Debug.Log("delete method called");

        if(selectedLevelButtonProperties)
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

    public void EditMoveCountText(int count)
    {
        moveCountText.text = count.ToString();

    }
    public void InstantiateSelectedLevel()
    {
        DeselectButton();
        selectedLevelButtonProperties.GetDeselected();
        selectedLevelButtonProperties.InstantiateLevel();
        GameManager.instance.StartLevel();
    }
}