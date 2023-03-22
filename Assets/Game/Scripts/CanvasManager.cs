using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    [SerializeField] private List<Canvas> canvasList;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private LevelButtonManager levelButtonManager;
    [SerializeField] private int level;
    private LevelBrain _levelBrain;
    private GameManager _gameManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }

    public enum PanelType
    {
        menu,
        levels,
        game,
        win,
        fail,
        loading
    }

    
    public void Init(LevelBrain levelBrain, GameManager gameManager)
    {
        _levelBrain = levelBrain;
        _gameManager = gameManager;

        SubscribeEvents();

        UpdateLevel();
        //levelText.text = "Level " + level;

        ActivateCanvas(PanelType.menu);
        levelButtonManager.PlayEntryAnimations();
    }

    private void UpdateLevel()
    {
        level = PlayerPrefs.GetInt("Level", 1);
        levelButtonManager.Init(level);
    }

    public void SubscribeEvents()
    {
        GameManager.instance.LevelStartedEvent += LevelStartedEvent;
        GameManager.instance.LevelFailedEvent += LevelFailedEvent;
        GameManager.instance.LevelSuccessEvent += LevelSuccessEvent;
    }

    public void UnSubscribeEvents()
    {
        GameManager.instance.LevelStartedEvent -= LevelStartedEvent;
        GameManager.instance.LevelFailedEvent -= LevelFailedEvent;
        GameManager.instance.LevelSuccessEvent -= LevelSuccessEvent;
    }

    private void LevelStartedEvent()
    {
        ActivateCanvas(PanelType.game);
    }

    public void ActivateCanvas(PanelType panelId)
    {
        int index = (int)panelId;

        for (int i = 0; i < canvasList.Count; i++)
        {
            if (i == index)
            {
                canvasList[i].gameObject.SetActive(true);
            }

            else
            {
                canvasList[i].gameObject.SetActive(false);
            }
        }
    }

    public void ActivateLevelCanvas()
    {
        UpdateLevel();
        ActivateCanvas(PanelType.levels);
        levelButtonManager.DeletePreviousLevel();
    }

    public void ActivateLoadingScreen()
    {
        ActivateCanvas(PanelType.loading);
    }
    private void LevelFailedEvent()
    {
        ActivateCanvas(PanelType.fail);
    }

    public void NextButton()
    {
        levelButtonManager.PrepareNextLevel();
        //Debug.Log("next button presed");
    }

    public void RetryButton()
    {
        levelButtonManager.Reset();
    }

    private void LevelSuccessEvent()
    {
        ActivateCanvas(PanelType.win);
        Debug.Log("level=" + level);
        int maxLevel = PlayerPrefs.GetInt("Level", level);
        int currentLevel = levelButtonManager.GetCurrentLevel();
        if (currentLevel == maxLevel) level += 1;

        if (level > maxLevel) PlayerPrefs.SetInt("Level", level);

        Debug.Log("level=" + level);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        if (level > 2)
        {
            int levelNumber = Random.Range(0, 3);
            SceneManager.LoadScene(levelNumber);
            level++;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            level++;
        }

        PlayerPrefs.SetInt("Level", level);
    }
}