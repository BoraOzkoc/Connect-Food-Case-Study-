using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int moveCount;
    [SerializeField] private bool isRandomGenerated;
    public void Init()
    {
        gridManager.Init(isRandomGenerated);
    }

    public void CreateManualGrids()
    {
        
    }
    public bool IsRandom()
    {
        return isRandomGenerated;
    }
    public int GetMoveCount()
    {
        return moveCount;
    }
}
