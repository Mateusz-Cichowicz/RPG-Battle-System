using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 3.0f;

    void Update()
    {
        //Input fields
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //simple camera movement
        transform.Translate(new Vector3(horizontalInput, 0.0f, verticalInput) * speed * Time.deltaTime, Space.World);
    }
}
