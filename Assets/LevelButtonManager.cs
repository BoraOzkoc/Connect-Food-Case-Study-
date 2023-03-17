using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private List<LevelButtonProperties> levelButtonList = new List<LevelButtonProperties>();
    [SerializeField] private List<LevelController> levelList = new List<LevelController>();

    public void Init(int savedLevel = 0)
    {
        for (int i = 0; i < levelButtonList.Count; i++)
        {
            bool state = savedLevel > i;

            levelButtonList[i].Init(i + 1, state, levelList[i]);
        }
    }
}
