using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTrap : MonoBehaviour
{
    [HideInInspector] public Vector3 startPos;
    public Vector3 endPos;
    [HideInInspector] public Quaternion startRot;
    public Quaternion endRot;
    public float startOffset;
    public float extendSpeed;
    public float waitTime;
    public float retractSpeed;
    public float loopTime;
    public bool rotate;

    private void OnValidate()
    {
        if (startOffset < 0) startOffset = 0;
        if (extendSpeed < 0) extendSpeed = 0;
        if (waitTime < 0) waitTime = 0;
        if (retractSpeed < 0) retractSpeed = 0;
        if (loopTime < startOffset + extendSpeed + waitTime + retractSpeed) loopTime = startOffset + extendSpeed + waitTime + retractSpeed;
    }

    float count;
    float Count
    {
        get { return count; }
        set
        {
            count = value;
            if (count >= loopTime) count -= loopTime;
            if (Count > startOffset && count <= startOffset + extendSpeed) Extend();
            else if (count >= startOffset + extendSpeed + waitTime && count <= startOffset + extendSpeed + waitTime + retractSpeed) Retract();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Count += Time.deltaTime;
    }

    void Extend()
    {
        float t = (Count - startOffset) / extendSpeed;
        if (rotate) transform.rotation = Quaternion.Lerp(startRot, Quaternion.Euler(startRot.eulerAngles + endRot.eulerAngles), t);
        else transform.position = Vector3.Lerp(startPos, startPos + endPos, t);
    }

    void Retract()
    {
        float t = (Count - startOffset - extendSpeed - waitTime) / retractSpeed;
        if (rotate) transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRot.eulerAngles + endRot.eulerAngles), startRot, t);
        else transform.position = Vector3.Lerp(startPos + endPos, startPos, t);
    }
}
