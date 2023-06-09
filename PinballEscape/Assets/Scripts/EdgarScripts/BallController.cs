using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    #region Variables
    private Rigidbody rb;
    private float dir = 0; // 1)right / -1)left

    public GameObject uiObject;
    public float sideAcceleration = 5f;
    public float maxSpeed = 50f;
    #endregion
    #region Built-In
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.right * dir * sideAcceleration);

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            print("slow speed");
        }

        Debug.DrawLine(transform.position, transform.position + rb.velocity);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<RayReceiver>(out RayReceiver r))
        {
            r.SendPoint(collision.GetContact(0).point);
            if(!r.rippleActive)
                StartCoroutine(r.Ripple());
            else
            {
                StopCoroutine(r.Ripple());
                r.rippleActive = false;
                StartCoroutine(r.Ripple());
            }
        }
    }
    #endregion
    #region InputSystem
    private void OnMove(InputValue val)
    {
        dir = val.Get<Vector2>().x;
    }
    #endregion
    #region Getters
    public Rigidbody RB
    {
        get { return rb; }
    }
    #endregion
    #region Functions
    #endregion
}
