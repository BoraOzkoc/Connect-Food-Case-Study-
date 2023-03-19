using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LevelButtonManager levelButtonManager;
    private List<GridController> selectedGridList = new List<GridController>();

    public void Init(InputManager inputManager)
    {
        inputManager.TouchStartedEvent += TouchStartedEvent;
        inputManager.TouchContinueEvent += TouchContinueEvent;
        inputManager.TouchEndedEvent += TouchEndedEvent;
    }

    private void TouchStartedEvent()
    {
        DrawRay();
    }

    private void TouchContinueEvent()
    {
        DrawRay();
    }

    private void TouchEndedEvent()
    {
        StopRay();
    }

    private void StopRay()
    {
        int listCount = selectedGridList.Count;
        if (listCount > 0 && listCount < 3)
        {
            for (int i = 0; i < selectedGridList.Count; i++)
            {
                selectedGridList[i].GetDeSelected();
            }
        }
        else if (listCount >= 3)
        {
            for (int i = 0; i < selectedGridList.Count; i++)
            {
                selectedGridList[i].GetDestroyed();
            }
            levelButtonManager.DecreaseCount();

        }
        selectedGridList.Clear();

    }
    private void DrawRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 50f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(cam.transform.position, mousePos - cam.transform.position, Color.blue);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.TryGetComponent(out GridController gridController))
            {
                if (selectedGridList.Count == 0)
                {
                    selectedGridList.Add(gridController.GetSelected());
                }
                else
                {
                    if (!gridController.IsSelected() && selectedGridList[^1].IsNeighbor(gridController) && selectedGridList[^1].IsSameType(gridController))
                    {
                        selectedGridList.Add(gridController.GetSelected());
                    }
                    else
                    {
                        Debug.Log("is not neighbor OR is selected");
                    }
                }
            }
        }
    }
}