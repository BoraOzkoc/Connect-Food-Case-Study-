using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();
    [SerializeField] private Image image;

    [SerializeField] private GameObject selectionFrame;
    [SerializeField] private List<GridController> neighborGridList = new List<GridController>();
    [SerializeField] private GameObject mesh;
    [SerializeField] private int _id;
    [SerializeField] private ParticleSystem spawnEffect;
    private bool _isSelected;

    public void Init(int order)
    {
        gameObject.name = "Grid_" + order;

        GetDeSelected();

        ChangeProperties();
    }


    private void ChangeProperties()
    {
        int randomNum = Random.Range(0, spriteList.Count);
        _id = randomNum;
        image.sprite = spriteList[_id];

        PlaySpawnEffect();
    }

    private void PlaySpawnEffect()
    {
        spawnEffect.Play();

        transform.DOScale(Vector3.one * 1.3f, 0.15f).SetEase(Ease.InQuint).SetLoops(2, LoopType.Yoyo);
    }

    public void AddNeighbor(GridController tempGrid)
    {
        neighborGridList.Add(tempGrid);
    }

    public GridController GetSelected()
    {
        _isSelected = true;
        selectionFrame.SetActive(true);
        return this;
    }

    public int GetID()
    {
        return _id;
    }

    public bool IsSameType(GridController gridController)
    {
        if (gridController.GetID() == _id) return true;
        return false;
    }

    public bool IsNeighbor(GridController tempGrid)
    {
        for (int i = 0; i < neighborGridList.Count; i++)
        {
            if (neighborGridList[i] == tempGrid) return true;
        }

        return false;
    }

    public void GetDeSelected()
    {
        _isSelected = false;
        selectionFrame.SetActive(false);
    }

    public void GetDestroyed()
    {
        IEnumerator DestroyCoroutine()
        {
            // gain resource
            // decrease move count
            // play destroy effect
            // make invisible for couple seconds
            mesh.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            // reset
            GetDeSelected();
            // change properties
            ChangeProperties();
            // make visible again
            mesh.SetActive(true);
            // play spawn effect
            PlaySpawnEffect();
        }

        StartCoroutine(DestroyCoroutine());
    }

    public bool IsSelected()
    {
        return _isSelected;
    }
}