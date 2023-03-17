using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private int count_x, count_z;
    [SerializeField] private float padding_x, padding_z;

    public void Init()
    {
        Vector3 startPos = transform.position;
        Vector3 currentPos = startPos;
        for (int i = 0; i < count_z; i++)
        {
            currentPos.x = startPos.x;
            currentPos += (Vector3.forward * padding_z);

            for (int j = 0; j < count_x; j++)
            {
                currentPos += (Vector3.right * padding_x);

                Instantiate(gridController, currentPos, Quaternion.identity);
            }
        }
    }
}