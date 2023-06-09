using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool bounce = true;
    public float bounceCoefficient = 1f;
    public float minimumBounce = 20f;
    [SerializeField] string audioClipName;
    AudioClipManager clipManager;

    ContactPoint point;
    private void Start()
    {
        clipManager = FindObjectOfType<AudioClipManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<BallController>(out BallController b))
        {
            point = collision.GetContact(0);
            if (bounce)
            {
                if (b.RB.velocity.magnitude > minimumBounce)
                    b.RB.velocity = -point.normal * (b.RB.velocity.magnitude * bounceCoefficient);
                else
                    b.RB.velocity = -point.normal * minimumBounce;
                if (audioClipName != "" && clipManager != null) clipManager.PlayClip(audioClipName); 
            }
        }
    }

}   
