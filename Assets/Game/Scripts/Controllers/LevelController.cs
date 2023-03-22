using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private int moveCount;
    [SerializeField] private bool levelAlreadyCreated;
    [SerializeField] private int[] itemCountList = new int[7];
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
    
    public int[] GetItemCountList()
    {
        return itemCountList;
    }
    public int GetMoveCount()
    {
        return moveCount;
    }
}