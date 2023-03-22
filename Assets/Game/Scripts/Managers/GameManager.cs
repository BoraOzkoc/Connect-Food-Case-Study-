using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event System.Action LevelStartedEvent;
    public event System.Action LevelSuccessEvent;
    public event System.Action LevelFailedEvent;

    private bool isLevelActive;

    private LevelBrain _levelBrain;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;

        }
    }
    public void Init(LevelBrain levelBrain)
    {
        _levelBrain = levelBrain;

    }

    public void DeActivateLevel()
    {
        isLevelActive = false;
    }
    public bool IsLevelActive()
    {
        return isLevelActive;
    }
    public void StartLevel()
    {
        LevelStartedEvent?.Invoke();
        isLevelActive = true;
    }
    public void EndGame(bool state)
    {
        

        if (state)
        {
            isLevelActive = false;
            LevelSuccessEvent?.Invoke();

        }
        else
        {
            if(isLevelActive) LevelFailedEvent?.Invoke();

        }
    }
}
