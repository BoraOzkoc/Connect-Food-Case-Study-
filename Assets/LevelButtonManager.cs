using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private List<LevelButtonProperties> levelList = new List<LevelButtonProperties>();

    public void Init()
    {
        for (int i = 0; i < levelList.Count; i++)
        {
            levelList[i].Init(i);
        }
    }
}
