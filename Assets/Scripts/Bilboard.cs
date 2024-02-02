using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bilboard : MonoBehaviour
{
    private Transform cam;
    private void Start()
    {
        cam = FindObjectOfType<Camera>().transform;

        if (cam == null)
        {
            Debug.LogError("Camera not found. Make sure a camera is present in the scene.");
        }
    }
    void LateUpdate()
    {
        if (cam != null)
        {
            transform.rotation = Quaternion.LookRotation(cam.forward);
        }
    }
}
