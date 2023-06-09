using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLightChanger : MonoBehaviour
{
    [SerializeField] Color insideColor;
    [SerializeField] Color outsideColor;
    [SerializeField] float changeDuration;

    [SerializeField] Light worldLight;

    BallController player;

    private void Start()
    {
        player = FindObjectOfType<BallController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject) StartLightChange(outsideColor);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player.gameObject) StartLightChange(insideColor);
    }

    void StartLightChange(Color color)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeLight(color));
    }

    IEnumerator ChangeLight(Color color)
    {
        Color startColor = worldLight.color;
        float startTime = Time.time;
        while (Time.time - startTime < changeDuration)
        {
            worldLight.color = Color.Lerp(startColor, color, (Time.time - startTime) / changeDuration);
            yield return null;
        }
    }
}
