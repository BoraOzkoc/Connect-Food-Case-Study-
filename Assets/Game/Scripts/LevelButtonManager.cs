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
    [SerializeField] private TextMeshProUGUI playButtonText;
    private LevelButtonProperties selectedLevelButtonProperties;

    public void Init(int savedLevel = 0)
    {
        DeselectButton();
        for (int i = 0; i < levelButtonList.Count; i++)
        {
            bool state = savedLevel > i;

            levelButtonList[i].Init(i + 1, state, levelList[i], this);
        }
    }

    private void DeselectButton()
    {
        playButton.interactable = false;
        TogglePlayButtonText(false);

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
        if(selectedLevelButtonProperties && selectedLevelButtonProperties != tempLevel)selectedLevelButtonProperties.GetDeselected();
        selectedLevelButtonProperties = tempLevel;
        selectedLevelButtonProperties.GetSelected();
        playButton.interactable = true;
        TogglePlayButtonText(true);
    }

    public void InstantiateSelectedLevel()
    {
        selectedLevelButtonProperties.InstantiateLevel();
        GameManager.instance.StartLevel();
    }
}