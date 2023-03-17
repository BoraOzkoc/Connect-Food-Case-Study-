using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelBrain : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private InputManager inputManager;

    private void Start()
    {
        gameManager.Init(this);
        canvasManager.Init(this);
        cameraManager.Init(this);
        inputManager.Init(this);
    }
}
