using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public bool firstTouch;
    public bool isTouching;
    public float initialTouch_y, initialTouch_x;
    public float lastTouch_y, lastTouch_x;
    public float delta_y, delta_x;
    public event System.Action TouchStartedEvent;
    public event System.Action TouchContinueEvent;
    public event System.Action TouchEndedEvent;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (!firstTouch)
            {
                firstTouch = true;
                GameManager.instance.StartGame();

            }

            isTouching = true;
            initialTouch_y = Input.mousePosition.y;
            initialTouch_x = Input.mousePosition.x;

            TouchStartedEvent?.Invoke();

        }
        if (Input.GetMouseButton(0))
        {

            lastTouch_y = Input.mousePosition.y;
            lastTouch_x = Input.mousePosition.x;

            delta_y = lastTouch_y - initialTouch_y;
            delta_x = lastTouch_x - initialTouch_x;

            initialTouch_y = Input.mousePosition.y;
            initialTouch_x = Input.mousePosition.x;

            TouchContinueEvent?.Invoke();

        }
        if (Input.GetMouseButtonUp(0))
        {

            isTouching = false;
            delta_y = 0f;
            delta_x = 0f;
            TouchEndedEvent?.Invoke();

        }
    }
    public Vector3 GetTouchPosition()
    {
        return new Vector3(initialTouch_x, initialTouch_y, 0);
    }
}
