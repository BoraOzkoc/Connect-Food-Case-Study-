using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    [SerializeField] private Camera cam;
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
    }


    private void DrawRay()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 50f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(cam.transform.position, mousePos - cam.transform.position, Color.blue);
    }
}