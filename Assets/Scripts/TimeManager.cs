using UnityEngine;
using UnityEngine.UI;  

public class TimeManager : MonoBehaviour
{
    public Text timeText;   
    private float timeRemaining = 300f;  

    void Start()
    {
        UpdateTimeDisplay();
    }

    void Update()
    {
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    void UpdateTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}