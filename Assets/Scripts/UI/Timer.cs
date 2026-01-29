using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelObjectivesText;

    private TextMeshProUGUI timeText;
    public TextMeshProUGUI TimeText
    {
        get { return timeText; }
    }


    private float startTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        startTime = Time.time;

        StartCoroutine(UpdateTimerRoutine());
    }

    IEnumerator UpdateTimerRoutine()
    {
        while (true)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timeText.text = minutes + ":" + seconds;

            yield return new WaitForSeconds(1);
        }
    }
}
