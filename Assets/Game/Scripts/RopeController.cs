using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    [SerializeField] private Transform ropeStartPoint, ropeEndPoint;
    
    public void AttachStartPoint(Vector3 startPos)
    {
        gameObject.transform.position = startPos;
        ropeStartPoint.position = startPos;
    }

    public void AttachEndPoint(Vector3 endPos)
    {
        ropeEndPoint.position = endPos;

    }

    public void ToggleRope(bool state)
    {
        gameObject.SetActive(state);
    }
    public void DestroyRope()
    {
        
    }

    public void MoveEndPoint(Vector3 pos)
    {
        ropeEndPoint.position = pos;
    }
    public void ResetRope(Vector3 pos)
    {
        
    }
}
