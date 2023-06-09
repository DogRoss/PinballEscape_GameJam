using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayReceiver : MonoBehaviour
{
    public float rippleTimer;

    public float rippleAmount;
    public float calmDownTimer;
    public Material m;

    [HideInInspector]
    public bool rippleActive = false;
    void Start()
    {
        if(m == null)
        {
            Debug.LogError("ERROR: no SHADER or MATERIAL implemented");
        }
    }

    public void SendPoint(Vector3 point)
    {
        m.SetVector("_RippleOrigin", point);
    }

    [ContextMenu("Ripple")]
    public void StartRip()
    {
        StartCoroutine(Ripple());
    }
    public IEnumerator Ripple()
    {
        rippleActive = true;
        m.SetFloat("_Amplitude", rippleAmount);
        yield return new WaitForSeconds(rippleTimer);

        float current = 0;
        while(current < calmDownTimer)
        {
            current += Time.deltaTime;
            m.SetFloat("_Amplitude", Mathf.Lerp(rippleAmount, 0, current / calmDownTimer));
            yield return null;
        }

        rippleActive = false;
        yield return null;
    }
}
