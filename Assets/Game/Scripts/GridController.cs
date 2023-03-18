using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private Image image;

    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private List<GridController> neighborGridList = new List<GridController>();
    private int _id;

    public void Init()
    {
        GetDeSelected();
        int randomNum = Random.Range(0, spriteList.Count);
        _id = randomNum;
        image.sprite = spriteList[_id];
    }

    public void AddNeighbor(GridController tempGrid)
    {
        neighborGridList.Add(tempGrid);
    }
    public void GetSelected()
    {
        
    }

    public void GetDeSelected()
    {
        selectionFrame.SetActive(false);
    }

    public void GetDestroyed()
    {
        
    }
}