using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LevelButtonManager levelButtonManager;
    [SerializeField] private RopePool ropePool;
    private List<GridController> selectedGridList = new List<GridController>();
    private List<RopeController> activeRopeList = new List<RopeController>();
    private InputManager _inputManager;
    private RopeController _activeRope;

    public void Init(InputManager inputManager)
    {
        _inputManager = inputManager;
        //SubscribeToTouchEvents();
    }

    public void SubscribeToTouchEvents()
    {
        //Debug.Log("subscribed");

        _inputManager.TouchStartedEvent += TouchStartedEvent;
        _inputManager.TouchContinueEvent += TouchContinueEvent;
        _inputManager.TouchEndedEvent += TouchEndedEvent;
    }

    public void UnsubscribeFromTouchEvents()
    {
        //Debug.Log("unsubscribed");
        _inputManager.TouchStartedEvent -= TouchStartedEvent;
        _inputManager.TouchContinueEvent -= TouchContinueEvent;
        _inputManager.TouchEndedEvent -= TouchEndedEvent;
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
        ClearActiveRopeList();
    }

    private void StopRay()
    {
        int listCount = selectedGridList.Count;
        if (listCount > 0 && listCount < 3)
        {
            for (int i = 0; i < listCount; i++)
            {
                selectedGridList[i].GetDeSelected();
            }
        }
        else if (listCount >= 3)
        {
            levelButtonManager.CheckLevelGoals(selectedGridList[0].GetID(), listCount);

            for (int i = 0; i < listCount; i++)
            {
                selectedGridList[i].GetDestroyed();
            }

            if (GameManager.instance.IsLevelActive()) levelButtonManager.DecreaseCount();
        }

        selectedGridList.Clear();
    }

    private void DrawRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 25;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(cam.transform.position, mousePos - cam.transform.position, Color.blue);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 25))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.TryGetComponent(out GridController gridController))
            {
                if (selectedGridList.Count == 0)
                {
                    selectedGridList.Add(gridController.GetSelected());
                    Vector3 gridPos = selectedGridList[^1].transform.position;
                    RopeController tempRope = ropePool.GetRopeFromPool();
                    tempRope.AttachStartPoint(gridPos);
                    _activeRope = tempRope;
                    activeRopeList.Add(_activeRope);
                }
                else
                {
                    if (!gridController.IsSelected() && selectedGridList[^1].IsNeighbor(gridController) &&
                        selectedGridList[^1].IsSameType(gridController))
                    {
                        selectedGridList.Add(gridController.GetSelected());

                        Vector3 gridPos = selectedGridList[^1].transform.position;
                        _activeRope.AttachEndPoint(gridPos);

                        RopeController tempRope = ropePool.GetRopeFromPool();
                        tempRope.AttachStartPoint(gridPos);
                        _activeRope = tempRope;
                        activeRopeList.Add(_activeRope);
                    }
                    else
                    {
                        //Debug.Log("is not neighbor OR is selected");
                    }
                }
            }
        }

        if (_activeRope)
        {
            _activeRope.MoveEndPoint(ray.GetPoint(20));
        }
    }

    private void ClearActiveRopeList()
    {
        for (int i = 0; i < activeRopeList.Count; i++)
        {
            ropePool.PushRopeToPool(activeRopeList[i]);
        }

        activeRopeList.Clear();
    }
}