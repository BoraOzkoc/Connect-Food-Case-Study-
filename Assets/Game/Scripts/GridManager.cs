using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private int count_x, count_z;
    [SerializeField] private float padding_x, padding_z;
    private List<GridController> gridList = new List<GridController>();
    private List<List<GridController>> listOfGridList = new List<List<GridController>>();

    public void Init(bool isRandom)
    {
        ConfigureLists();

        CreateGrids();

        SetNeighbors();

        ActivateGrids(isRandom);
    }

   
    private void ConfigureLists()
    {
        for (int i = 0; i < count_z; i++)
        {
            List<GridController> gridList = new List<GridController>();
            listOfGridList.Add(gridList);
        }
    }

    private void CreateGrids()
    {
        Vector3 startPos = transform.position;
        Vector3 currentPos = startPos;

        GameObject parent = new GameObject("Grid Template");

        for (int i = 0; i < count_z; i++)
        {
            currentPos.x = startPos.x;
            currentPos += (Vector3.forward * padding_z);

            for (int j = 0; j < count_x; j++)
            {
                currentPos += (Vector3.right * padding_x);

                GridController tempGrid =
                    Instantiate(gridController, currentPos, Quaternion.identity, parent.transform);

                listOfGridList[i].Add(tempGrid);
            }
        }
    }

    private void SetNeighbors()
    {
        for (int i = 0; i < listOfGridList.Count; i++)
        {
            for (int j = 0; j < listOfGridList[i].Count; j++)
            {
                GridController currentGrid = listOfGridList[i][j];

                if (j != 0) // is it first of the current list
                {
                    currentGrid.AddNeighbor(listOfGridList[i][j - 1]);
                }

                if (j != listOfGridList[i].Count - 1) // is it last of the current list
                {
                    currentGrid.AddNeighbor(listOfGridList[i][j + 1]);
                }

                if (i != 0) // is it first list of list
                {
                    if (j != 0) // is it first of the current list
                    {
                        currentGrid.AddNeighbor(listOfGridList[i - 1][j - 1]);
                    }

                    if (j != listOfGridList[i].Count - 1) // is it last of the current list
                    {
                        currentGrid.AddNeighbor(listOfGridList[i - 1][j + 1]);
                    }

                    currentGrid.AddNeighbor(listOfGridList[i - 1][j]);
                }

                if (i != listOfGridList.Count - 1) // is it last list of list
                {
                    if (j != 0) // is it first of the current list
                    {
                        currentGrid.AddNeighbor(listOfGridList[i + 1][j - 1]);
                    }

                    if (j != listOfGridList[i].Count - 1) // is it last of the current list
                    {
                        currentGrid.AddNeighbor(listOfGridList[i + 1][j + 1]);
                    }

                    currentGrid.AddNeighbor(listOfGridList[i + 1][j]);
                }
            }
        }
    }

    private void ActivateGrids(bool isRandom)
    {
        int order = 0;
        for (int i = 0; i < listOfGridList.Count; i++)
        {
            for (int j = 0; j < listOfGridList[i].Count; j++)
            {
                listOfGridList[i][j].Init(order, isRandom);
                order++;
            }
        }
    }
}