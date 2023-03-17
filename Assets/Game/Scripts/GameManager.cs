using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event System.Action LevelStartedEvent;
    public event System.Action LevelSuccessEvent;
    public event System.Action LevelFailedEvent;

    public bool isLevelActive;

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
        // isLevelActive = true;
        // LevelStartedEvent?.Invoke();

    }
    public void EndGame(bool state)
    {
        if (state)
        {
            LevelSuccessEvent?.Invoke();

        }
        else
        {
            LevelFailedEvent?.Invoke();

        }
    }
}
