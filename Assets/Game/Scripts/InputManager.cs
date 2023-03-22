using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [SerializeField] private bool firstTouch;
    [SerializeField] private bool isTouching;
    [SerializeField] private float initialTouch_y, initialTouch_x;
    [SerializeField] private float lastTouch_y, lastTouch_x;
    [SerializeField] private float delta_y, delta_x;
    private RayCastController _rayCastController;
     public event System.Action TouchStartedEvent;
     public event System.Action TouchContinueEvent;
     public event System.Action TouchEndedEvent;
    private LevelBrain _levelBrain;
    public void Init(LevelBrain levelBrain)
    {
        _levelBrain = levelBrain;
        _rayCastController = GetComponent<RayCastController>();
        _rayCastController.Init(this);
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

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.IsLevelActive())
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (!firstTouch)
                {
                    firstTouch = true;

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
    }
    public Vector3 GetTouchPosition()
    {
        return new Vector3(initialTouch_x, initialTouch_y, 0);
    }
}
