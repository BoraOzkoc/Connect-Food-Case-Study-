using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int moveCount;
    [SerializeField] private bool levelAlreadyCreated;

    public void Init()
    {
        if(!levelAlreadyCreated)
        {
            gridManager.Init();
            levelAlreadyCreated = true;
        }
    }
    public void Reset()
    {
        levelAlreadyCreated = false;
    }

    public void CreateManualGrids()
    {
        
    }
    

    public int GetMoveCount()
    {
        return moveCount;
    }
}