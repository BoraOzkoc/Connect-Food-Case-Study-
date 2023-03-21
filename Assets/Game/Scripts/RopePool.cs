using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePool : MonoBehaviour
{
    [SerializeField] private RopeController ropePrefab;
    [SerializeField] private int poolSize;
    private Queue<RopeController> pooledRopes;

    private void Awake()
    {
        SpawnRopes();
    }

    private void SpawnRopes()
    {
        pooledRopes = new Queue<RopeController>();

        for (int i = 0; i < poolSize; i++)
        {
            RopeController rope = Instantiate(ropePrefab,transform);
            rope.ToggleRope(false);
            
            pooledRopes.Enqueue(rope);
        }
    }

    public RopeController GetRopeFromPool()
    {
        RopeController rope = pooledRopes.Dequeue();
        
        rope.ToggleRope(true);
        if (pooledRopes.Count <= 1)
        {
            SpawnRopes();
        }
        
        
        return rope;
    }

    public void PushRopeToPool(RopeController rope)
    {
        rope.ToggleRope(false);
        pooledRopes.Enqueue(rope);
    }
}
