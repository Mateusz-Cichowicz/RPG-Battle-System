using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 target;
    public float speed;
    public MyVector3Event ObjectDestroyed;
    void Update()
    {
        if (target != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // Check if the position is approximately equal to the target
            if (Vector3.Distance(transform.position, target) < 0.001f)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        ObjectDestroyed?.Invoke(transform.position);
    }
}
