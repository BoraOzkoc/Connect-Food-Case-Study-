using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelBrain : MonoBehaviour
{
    public int targetFrameRate = 60;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CanvasManager canvasManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private InputManager inputManager;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        
        gameManager.Init(this);
        canvasManager.Init(this, gameManager);
        cameraManager.Init(this);
        inputManager.Init(this);
    }

}
