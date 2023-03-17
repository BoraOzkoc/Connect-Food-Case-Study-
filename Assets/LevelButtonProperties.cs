using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButtonProperties : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    private int levelNumber;
    
    public void Init(int givenNumber)
    {
        levelNumber = givenNumber;
        SetLevelText();
    }

    private void SetLevelText()
    {
        levelText.text = levelNumber.ToString();
    }
}
