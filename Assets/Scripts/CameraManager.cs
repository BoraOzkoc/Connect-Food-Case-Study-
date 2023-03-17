using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera gameCam, menuCam;
    public Camera cam;
    private void Start()
    {
        GameManager.instance.LevelStartedEvent += _LevelStartedEvent;
        ActivateCamera(menuCam);
    }

    private void _LevelStartedEvent()
    {
        ActivateCamera(gameCam);

    }
    private void ActivateCamera(CinemachineVirtualCamera virtualCam)
    {
        if (virtualCam == menuCam)
        {
            menuCam.Priority = 50;
            gameCam.Priority = 0;
        }
        else if(virtualCam == gameCam)
        {
            menuCam.Priority = 0;
            gameCam.Priority = 50;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }
}
