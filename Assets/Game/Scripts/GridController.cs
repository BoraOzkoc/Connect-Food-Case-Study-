using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private Sprite sprite;

    [SerializeField] private GameObject selectionFrame;
    private int _id;

    public void Init()
    {
        GetDeSelected();
        int randomNum = Random.Range(0, spriteList.Count);
        _id = randomNum;
        sprite = spriteList[_id];
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