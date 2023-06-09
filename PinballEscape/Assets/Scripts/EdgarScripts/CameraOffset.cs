using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraOffset : MonoBehaviour
{
    public Transform obj;
    public Vector3 offset;

    public bool lookAt = true;
    public bool followObj = true;
    public float snappiness = 1f;

    void Start()
    {
        transform.position = obj.position + offset;
    }

    private void FixedUpdate()
    {
        if(followObj)
            transform.position = Vector3.Lerp(transform.position, obj.position + offset, snappiness * Time.deltaTime);

        if (lookAt)
            transform.LookAt(obj.position);
    }
}
