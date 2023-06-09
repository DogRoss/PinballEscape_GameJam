using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] float acceleration;
    
    BallController ball;
    Rigidbody rb;
    bool conveying;
    
    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<BallController>();
        rb = ball.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (conveying)
        {
            rb.velocity += transform.up * acceleration;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ball.gameObject)
        {
            conveying = true;
            ball.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ball.gameObject)
        {
            conveying = false;
            ball.enabled = true;
        }
    }
}
