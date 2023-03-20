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
        fail
        
    }

    public void Init(LevelBrain levelBrain,GameManager gameManager)
    {
        _levelBrain = levelBrain;
        _gameManager = gameManager;

        GameManager.instance.LevelStartedEvent += LevelStartedEvent;
        GameManager.instance.LevelFailedEvent += LevelFailedEvent;
        GameManager.instance.LevelSuccessEvent += LevelSuccessEvent;
        // level = PlayerPrefs.GetInt("Level", 1);
        levelButtonManager.Init(level);
        //levelText.text = "Level " + level;

        ActivateCanvas(PanelType.menu);
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
        ActivateCanvas(PanelType.levels);
        levelButtonManager.DeletePreviousLevel();
        
    }

    private void LevelFailedEvent()
    {
        ActivateCanvas(PanelType.fail);

    }

    public void RetryButton()
    {
        levelButtonManager.Reset();

    }
    private void LevelSuccessEvent()
    {
        ActivateCanvas(PanelType.win);
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