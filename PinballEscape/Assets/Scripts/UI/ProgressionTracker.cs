using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionTracker : MonoBehaviour
{
    [SerializeField] GameObject levelStart;
    [SerializeField] GameObject levelEnd;
    [SerializeField] Text timeText;
    [SerializeField] Image whiteScreen;
    [SerializeField] float fadeToWhiteDuration;
    BallController player;
    CameraOffset camera;

    float distanceFromStartToEnd;
    float startTime;

    [SerializeField] Slider progressionSlider;

    private void Start()
    {
        player = FindObjectOfType<BallController>();
        camera = FindObjectOfType<CameraOffset>();
        distanceFromStartToEnd = Vector3.Distance(levelStart.transform.position, levelEnd.transform.position);
        if (progressionSlider != null) progressionSlider.value = Vector3.Distance(player.transform.position, levelStart.transform.position) / distanceFromStartToEnd;
        startTime = Time.time;
    }

    private void Update()
    {
        if (progressionSlider != null) progressionSlider.value = Vector3.Distance(player.transform.position, levelStart.transform.position) / distanceFromStartToEnd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            if (timeText != null)
            {
                timeText.text = "Escape Time: " + (Time.time - startTime).ToString();
                timeText.gameObject.SetActive(true);
            }
            camera.followObj = false;
            StartCoroutine(FadeToWhite());
        }
    }

    IEnumerator FadeToWhite()
    {
        float tempTime = Time.time;
        while ((Time.time - tempTime) < fadeToWhiteDuration)
        {
            whiteScreen.color = Color.Lerp(Color.clear, Color.white, (Time.time - tempTime) / fadeToWhiteDuration);
            yield return null;
        }
        SceneChanger.instance.ChangeScene(0);
    }
}
