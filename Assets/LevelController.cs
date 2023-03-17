using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    public void Init()
    {
        gridManager.Init();
    }
}
