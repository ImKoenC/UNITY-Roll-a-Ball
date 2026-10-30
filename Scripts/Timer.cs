using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI timerText;

    private float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        currentTime = 0f; // Start stopwatch at 0
        isTimerRunning = true;
    }

    void Update()
    {
        if (!isTimerRunning) return;

        // Count upwards for a stopwatch
        currentTime += Time.deltaTime;
        DisplayTime(currentTime);
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        
        // Optional: Uncomment the lines below if you want milliseconds for a stopwatch
        // float milliseconds = Mathf.FloorToInt((timeToDisplay % 1) * 100);
        // timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Call this function from another script to freeze the stopwatch
    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Stopwatch stopped at: " + timerText.text);
    }
}
