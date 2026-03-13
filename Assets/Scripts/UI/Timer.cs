using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private Coroutine timerOffcoroutine;
    private TextMeshProUGUI timeText;
    public TextMeshProUGUI TimeText
    {
        get { return timeText; }
    }


    private float startTime;
    private float endTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
        startTime = Time.time;

        timerOffcoroutine = StartCoroutine(UpdateTimerRoutine());
    }

    IEnumerator UpdateTimerRoutine()
    {
        while (true)
        {
            float t = Time.time - startTime;
            endTime = t;
            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");

            timeText.text = minutes + ":" + seconds;

            yield return new WaitForSeconds(1);
        }
    }

    public void TimerOff()
    {
        StopCoroutine(timerOffcoroutine);
        timerOffcoroutine = null;
    }

    public int TimeBonusMultiplier(int baseBonusTime)
    {
        int sec = baseBonusTime - (int)endTime;
        return sec > 0 ? sec : 0;
    }
}
