using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;
    public List<Canvas> canvasList;
    private int level;
    public TextMeshProUGUI levelText;

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
        game, fail, win, menu
    }
    private void Start()
    {
        GameManager.instance.LevelStartedEvent += LevelStartedEvent;
        GameManager.instance.LevelFailedEvent += LevelFailedEvent;
        GameManager.instance.LevelSuccessEvent += LevelSuccessEvent;
        level = PlayerPrefs.GetInt("Level", 1);
        levelText.text = "Level " + level;

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
    private void LevelFailedEvent()
    {
        ActivateCanvas(PanelType.fail);
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
            int levelNumber = Random.Range(0,3);
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
